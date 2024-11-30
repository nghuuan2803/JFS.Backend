using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JFS.Backend.Hubs
{
    //[Authorize]
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToUser(string userId, string message)
        {
            string? user_id = Context.UserIdentifier;
            await Clients.User(userId).SendAsync("ReceiveNotification", message + "from" + user_id);
        }

        public async Task SendNotificationToAll(string message)
        {
            string? user_id = Context.UserIdentifier;
            await Clients.All.SendAsync("ReceiveNotification", message + " from " + user_id);
        }
    }
}
