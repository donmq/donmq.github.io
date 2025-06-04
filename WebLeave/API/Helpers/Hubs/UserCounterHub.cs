using API.Helpers.Enums;
using Microsoft.AspNetCore.SignalR;

namespace API.Helpers.Hubs
{
    public class UserCounterHub : Hub
    {
        public static class ConnectedUsers
        {
            public static List<string> Ids = new List<string>();
        }
        private static int userCount;
        public override Task OnConnectedAsync()
        {
            ConnectedUsers.Ids.Add(Context.ConnectionId);
            userCount = ConnectedUsers.Ids.Count;
            base.OnConnectedAsync();
            Clients.All.SendAsync(CommonConstants.SR_CONNECTED_USERS, userCount);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.Ids.Remove(Context.ConnectionId);
            userCount = ConnectedUsers.Ids.Count;
            base.OnDisconnectedAsync(exception);
            Clients.All.SendAsync(CommonConstants.SR_CONNECTED_USERS, userCount);
            return Task.CompletedTask;
        }
    }
}