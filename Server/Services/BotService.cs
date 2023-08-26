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
        private readonly IHubContext<NotificationHub> hubContext;
        public BotService(OpenAIClient openAIClient, IHubContext<NotificationHub> hubContext)
        {
            this.openAIClient = openAIClient;
            this.hubContext = hubContext;
        }

        public async Task<bool> AnswerToMessageAsync(Chat chat, Message message)
        {
            var result = await openAIClient.SendPrompt(message.Text);

            if (await CheckIfMessageIsBotHandable(result) is false)
            {
                return false;
            }

            

            await Task.Delay(new Random().Next(2000, 4000));

            chat.Messages.Add(new Message { MessageType = MessageType.Question, Text = "AtemÜbungen", Answers = new System.Collections.Generic.List<string> { "Ja", "Nein" } });

            await hubContext.Clients.All.SendAsync("Update");

            return true;
        }

        private async Task<bool> CheckIfMessageIsBotHandable(string result)
        {
            return true;
        }
    }
}
