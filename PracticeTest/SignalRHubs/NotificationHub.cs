using BLL.DTO;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.SignalRHubs
{
    public class NotificationHub: Hub
    {
        public async Task Send(TraineeDTO message)
        {
            await Clients.All.SendAsync("ReceiveEdit", message);
        }
    }
}
