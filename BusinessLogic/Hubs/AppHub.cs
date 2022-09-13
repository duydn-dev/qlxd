using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BusinessLogic.Hubs
{
    public class AppHub : Hub
    {
        public async Task ClientTestEmit()
        {
            await Clients.All.SendAsync("ServerTestEmit", "Hello world");
        }
    }
}
