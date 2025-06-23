using AuthService.Domain.Models;
using MainService.Application.Interfaces;
using MainService.Domain.Models;
using MainService.Domain.Models.Streaming;
using MainService.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using File = MainService.Domain.Models.File;

namespace MainService.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(DbContextOptions options, ICurrentUser currentUser, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(options, currentUser, dbSettings, events)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Token> Token => Set<Token>();
    public DbSet<File> File => Set<File>();
    public DbSet<Meeting> Meeting => Set<Meeting>();
    public DbSet<MeetingMessage> Message => Set<MeetingMessage>();
    public DbSet<Recruitment> Recruitment => Set<Recruitment>();
    public DbSet<Profession> Profession => Set<Profession>();
    public DbSet<RecruitmentKeyword> RecruitmentTag => Set<RecruitmentKeyword>();
    public DbSet<Review> Review => Set<Review>();
    public DbSet<Keyword> Tag => Set<Keyword>();
    public DbSet<UserMeeting> UserMeeting => Set<UserMeeting>();
    public DbSet<UserRecruitment> UserRecruitment => Set<UserRecruitment>();
    public DbSet<Room> Room => Set<Room>();
    public DbSet<RoomUser> RoomUser => Set<RoomUser>();
    public DbSet<Notification> Notification => Set<Notification>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.App);
    }
}