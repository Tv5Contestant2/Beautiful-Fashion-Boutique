using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ECommerce1.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("newMessage", user, message);
        }
    }
}