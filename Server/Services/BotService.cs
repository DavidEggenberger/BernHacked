using Microsoft.AspNetCore.SignalR;
using Server.DomainFeatures.ChatAggregate.Domain;
using Server.Hubs;
using System;
using System.Threading.Tasks;

namespace Server.Services
{
    public class BotService
    {
        
        public BotService()
        {
        }

        public bool CheckIfMessageIsBotHandable(Message message)
        {

            return true;
        }

        public async Task AnswerToMessageAsync(Chat chat, Message message)
        {
            await Task.Delay(new Random().Next(2000, 4000));

            
        }
    }
}
