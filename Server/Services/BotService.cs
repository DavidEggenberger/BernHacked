using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Server.DomainFeatures.ChatAggregate.Domain;
using Server.Hubs;
using Server.Services.AzureSpeech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class BotService
    {
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly IConfiguration configuration;
        private readonly TextToSpeechService textService;
        public BotService(IHubContext<NotificationHub> hubContext, IConfiguration configuration, TextToSpeechService textService)
        {
            this.hubContext = hubContext;
            this.configuration = configuration;
            this.textService = textService;
        }

        public async Task<bool> AnswerToMessageAsync(Chat chat, Message message)
        {
            var api = new OpenAI_API.OpenAIAPI(configuration["OpenAI"]);
            var result = await api.Completions.GetCompletion("One Two Three One Two");

            var aiChat = api.Chat.CreateConversation();
            aiChat.AppendSystemMessage("Sie sind ein Psychiater spezialisiert auf junge, zwischen 18 und 25 jährige Erwachsene. " +
                "Probieren sie freundlich, präzise und prägnant zu antworten. " +
                "Die Antwort sollte höchsten 8 wörter lang sein.");

            aiChat.AppendUserInput("Was kannst du mir empfehlen?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");
            aiChat.AppendUserInput("Kannst du mir etwas empfehlen?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");
            aiChat.AppendUserInput("Was würdest du mir empfehlen?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");
            aiChat.AppendUserInput("Was würdest du tun?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");


            aiChat.AppendUserInput("Ich interessiere mich für Stressbewältigung");
            aiChat.AppendExampleChatbotOutput("Ich kann dir unseren Wiki Eintrag zur Stressbewältigung empfehlen");
            aiChat.AppendUserInput("Hast du Tipps zur Stressbewältigung");
            aiChat.AppendExampleChatbotOutput("Ich kann dir unseren Wiki Eintrag zur Stressbewältigung empfehlen");
            aiChat.AppendUserInput("Ich suche Tipps zur Stressbewältigung");
            aiChat.AppendExampleChatbotOutput("Ich kann dir unseren Wiki Eintrag zur Stressbewältigung empfehlen");


            aiChat.AppendUserInput(message.Text);
            
            if(message.Text == null)
            {
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Text, Text = "Auf ihre Anfrage findet der ChatBot leider keine Antwort" });

                await hubContext.Clients.All.SendAsync("Update");
                
                return false;
            }

            string response = await aiChat.GetResponseFromChatbotAsync();
            
            if (await CheckIfMessageIsBotHandable(result) is false)
            {
                return false;
            }

            var keyWords = new List<string> { "meditation", "atemübung", "achtsamkeitsübung " };
            if (keyWords.Any(x => response.ToLower().Contains(x)))
            {
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Text, Text = response });
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Question, Text = "Willst du eine Atem Übung machen?", Answers = new System.Collections.Generic.List<string> { "Ja", "Nein" } });
            }
            else
            {
                if (chat?.ChatOptions?.Any(x => x == ChatConfiguration.Speech) == true)
                {
                    var id = await textService.SynthesizeAudioAsync(response);
                    chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Voice, Text = id });
                }
                else
                {
                    chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Text, Text = response });
                }
            }

            await Task.Delay(new Random().Next(2000, 4000));

            await hubContext.Clients.All.SendAsync("Update");

            return true;
        }

        private async Task<bool> CheckIfMessageIsBotHandable(string result)
        {
            return true;
        }
    }
}
