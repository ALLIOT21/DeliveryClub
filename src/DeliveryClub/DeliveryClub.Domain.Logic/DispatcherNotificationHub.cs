using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic
{
    public class DispatcherNotificationHub : Hub
    {
        public async Task JoinGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
        }
    }
}
