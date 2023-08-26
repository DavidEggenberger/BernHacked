using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Server.DomainFeatures.ChatAggregate.Domain;
using Server.Hubs;
using System;
using System.Threading.Tasks;

namespace Server.Services
{
    public class BotService
    {
        private readonly OpenAIClient openAIClient;
        public BotService(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }

        public bool CheckIfMessageIsBotHandable(Message message)
        {

            return true;
        }

        public async Task AnswerToMessageAsync(Chat chat, Message message)
        {
            await Task.Delay(new Random().Next(2000, 4000));

            var result = await openAIClient.SendPrompt(message.Text);

            chat.Messages.Add(new Message { Text = result });
        }
    }
}
