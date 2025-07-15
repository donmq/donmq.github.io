using Microsoft.AspNetCore.SignalR;

namespace Machine_API.Helpers.Hubs
{
    public class HitCounter : Hub
    {
        public static class ConnectedUser
        {
            public static List<string> Ids = new List<string>();
        }
        private static int Count;
        public override Task OnConnectedAsync()
        {
            ConnectedUser.Ids.Add(Context.ConnectionId);
            Count = ConnectedUser.Ids.Count;
            base.OnConnectedAsync();
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUser.Ids.Remove(Context.ConnectionId);
            Count = ConnectedUser.Ids.Count;
            base.OnDisconnectedAsync(exception);
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
    }
}