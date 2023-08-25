using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        public async Task NotifyClient()
        {

        }
    }
}
