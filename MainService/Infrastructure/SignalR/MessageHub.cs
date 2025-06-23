using MainService.Application.Interfaces;
using MainService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.SignalR;

namespace MainService.Infrastructure.SignalR;

public class MessageHub(ApplicationDbContext dbContext, ICurrentUser currentUser) : Hub
{
}
