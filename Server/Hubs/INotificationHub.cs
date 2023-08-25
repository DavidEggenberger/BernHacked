using System.Threading.Tasks;

namespace Server.Hubs
{
    public interface INotificationHub
    {
        Task UpdateClients();
    }
}
