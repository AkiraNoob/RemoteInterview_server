using Core.Domain.Contracts;
using Core.MediatR.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Npgsql.NameTranslation;
using Shared.Application.Common.Interfaces;
using Shared.Infrastructure.Identity;
using System.Data;
using TMS.Infrastructure.Persistence;

namespace Shared.Infrastructure.Persistence.Context;

public abstract class BaseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
{
    protected readonly ICurrentUser _currentUser;
    private readonly DatabaseSettings _dbSettings;
    private readonly IEventPublisher _publisher;

    protected BaseDbContext(DbContextOptions options, ICurrentUser currentUser, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(options)
    {
        _currentUser = currentUser;
        _dbSettings = dbSettings.Value;
        _publisher = events;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userId = _currentUser.GetUserId();
        HandleAuditingBeforeSaveChanges(userId);
        var domainEvents = GetDomainEvents();
        int result = await base.SaveChangesAsync(cancellationToken);
        await SendDomainEventsAsync(domainEvents, cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        ApplySnakeCaseNames(modelBuilder);
    }

    private static void ApplySnakeCaseNames(ModelBuilder modelBuilder)
    {
        var mapper = new NpgsqlSnakeCaseNameTranslator();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                string npgsqlColumnName = mapper.TranslateMemberName(property.GetColumnName());

                property.SetColumnName(npgsqlColumnName);
            }

            entity.SetTableName(mapper.TranslateTypeName(entity.GetTableName()));
        }
    }

    private List<INotification> GetDomainEvents()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList();

        var domainEvents = new List<INotification>();

        foreach (var entry in entries)
        {
            object? entity = entry.Entity;
            var state = entry.State;
            if (entity is { } && state == EntityState.Added)
            {
                var domainEvent = (INotification)Activator.CreateInstance(typeof(EntityCreatedEvent<>).MakeGenericType(entity.GetType()), [entity])!;
                domainEvents.Add(domainEvent);
            }
            else if (entity is { } && state == EntityState.Modified)
            {
                var domainEvent = (INotification)Activator.CreateInstance(typeof(EntityUpdatedEvent<>).MakeGenericType(entity.GetType()), [entity])!;
                domainEvents.Add(domainEvent);
            }
            else if (entity is { } && state == EntityState.Deleted)
            {
                var domainEvent = (INotification)Activator.CreateInstance(typeof(EntityDeletedEvent<>).MakeGenericType(entity.GetType()), [entity])!;
                domainEvents.Add(domainEvent);
            }
        }

        return domainEvents;
    }

    private void HandleAuditingBeforeSaveChanges(Guid userId)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = userId;
                        softDelete.DeletedOn = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
        }

        ChangeTracker.DetectChanges();
    }

    private async Task SendDomainEventsAsync(List<INotification> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.PublishAsync(domainEvent, cancellationToken);
        }
    }
}