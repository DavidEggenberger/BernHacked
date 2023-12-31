﻿using Microsoft.AspNetCore.SignalR;
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

            aiChat.AppendUserInput("Ich bin wütend, was kannst du mir empfehlen?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");
            aiChat.AppendUserInput("Ich bin ängstlich, was kannst du mir empfehlen?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");
            aiChat.AppendUserInput("was kannst du für mein Wohlbefinden empfehlen?");
            aiChat.AppendExampleChatbotOutput("Ich kann dir eine Atemübung empfehlen");
            aiChat.AppendUserInput("Mich belastet vieles, was Kannst du mir empfehlen?");
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

            aiChat.AppendUserInput("Ich suche dringend Hilfe");
            aiChat.AppendExampleChatbotOutput("Ich bin als Chatbot nicht geeignet dir weiter zu helfen");
            aiChat.AppendUserInput("Ich bin dringend auf Hilfe angewiesen");
            aiChat.AppendExampleChatbotOutput("Ich bin als Chatbot nicht geeignet dir weiter zu helfen");
            aiChat.AppendUserInput("Hilfe sofort");
            aiChat.AppendExampleChatbotOutput("Ich bin als Chatbot nicht geeignet dir weiter zu helfen");

            aiChat.AppendUserInput(message.Text);

            var keywords3 = new List<string>() { "suche", "tipps", "geben", "zeigen", "tutorial", "gib", "zeig" };
            if (keywords3.Where(x => message?.Text?.ToLower().Contains(x) == true).Count() > 1)
            {
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Question, Text = "Darf ich dich in unser Wiki weiterleiten?", Answers = new List<string> { "Ja", "Nein" } });

                await hubContext.Clients.All.SendAsync("Update");

                return false;
            }


            if(message.Text == null)
            {
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Text, Text = "Auf ihre Anfrage findet der ChatBot leider keine Antwort" });

                await hubContext.Clients.All.SendAsync("Update");
                
                return false;
            }

            string response = await aiChat.GetResponseFromChatbotAsync();
            
            

            var keyWords = new List<string> { "meditation", "atemübung", "achtsamkeitsübung " };
            var keyWords1 = new List<string> { "kontaktiere", "fachleuten", "hotline", "nicht geeignet", "notruf", "professionalle", "beratungsstelle" };
            if (keyWords.Any(x => response.ToLower().Contains(x)))
            {
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Text, Text = response });
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.Question, Text = "Willst du eine Atem Übung machen?", Answers = new System.Collections.Generic.List<string> { "Ja", "Nein" } });
            }
            else if(keyWords1.Any(x => response.ToLower().Contains(x)))
            {
                chat.Messages.Add(new Message { Bot = true, MessageType = MessageType.SpecialInformation, Text = "Der Chatbot kann nicht weiterhelfen. Eine Person meldet sich schnellstmöglich." });
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

            await Task.Delay(new Random().Next(500, 1000));

            await hubContext.Clients.All.SendAsync("Update");

            return true;
        }

        private async Task<bool> CheckIfMessageIsBotHandable(string result)
        {
            return true;
        }
    }
}
