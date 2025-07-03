using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace eTierV2_API.Hubs
{
    public class ChangeRouterHub : Hub
    {
        public async Task SendMessage(string router)
        {
            await Clients.All.SendAsync("ReceiveMessage", router);
        }
    }
}