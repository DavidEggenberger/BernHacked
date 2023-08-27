using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.DomainFeatures.ChatAggregate.Domain;
using Server.DomainFeatures.ChatAggregate.Infrastructure;
using Server.Hubs;
using Server.Services;
using Shared.Chat;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ChatPersistence chatPersistence;
        private readonly RandomGenerator randomGenerator;
        private readonly BotService botService;
        public ChatsController(ChatPersistence chatPersistence, RandomGenerator randomGenerator, BotService botService)
        {
            this.chatPersistence = chatPersistence;
            this.randomGenerator = randomGenerator;
            this.botService = botService;
        }

        /// <summary>
        /// Retrieve a specified chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [HttpGet("{chatId}")]
        public ActionResult<ChatDTO> Get(int chatId)
        {
            return Ok(chatPersistence.Chats.Single(c => c.ChatId == chatId));
        }


        /// <summary>
        /// Create a new chat
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ChatDTO>> CreateChat(ChatDTO chatDTO)
        {
            chatDTO.Id = randomGenerator.GenerateRandomInt();

            foreach (var messages in chatDTO.Messages)
            {
                messages.SentAt = DateTime.Now;
            }

            var chat = Chat.FromDTO(chatDTO);

            chatPersistence.AddChat(chat);

            var message = chat.Messages.Last();
            if (await botService.AnswerToMessageAsync(chat, message))
            { 
            }

            return Ok(chat.ToDTO());
        }

        /// <summary>
        /// Create a new chat
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost("{chatId}/configuration")]
        public async Task<ActionResult<ChatDTO>> UpdateConfigurationOfChat([FromRoute] int chatId, [FromBody] ChatConfigurationContainer chatConfigurationDTOs)
        {
            Chat chat = chatPersistence.Chats.Single(x => x.ChatId == chatId);

            chat.ChatOptions = chatConfigurationDTOs.Configurations.Select(x => (ChatConfiguration)x).ToList();

            return Ok(chat.ToDTO());
        }

        /// <summary>
        /// Add a message to an existing chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="messageDTO"></param>
        /// <returns></returns>
        [Tags("Messages")]
        [HttpPost("{chatId}/messages")]
        public async Task<ActionResult> CreateMessageInChat([FromRoute] int chatId, MessageDTO messageDTO)
        {
            messageDTO.SentAt = DateTime.Now;

            Chat chat = chatPersistence.Chats.Single(x => x.ChatId == chatId);
            Message message = Message.FromDTO(messageDTO);

            chat.AddMessage(message);

            if (await botService.AnswerToMessageAsync(chat, message))
            {
            }

            return Ok();
        }

        /// <summary>
        /// Add a message to an existing chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [Tags("Messages")]
        [HttpGet("{chatId}/messages")]
        public ActionResult<IEnumerable<ChatDTO>> GetMessagesForChat([FromRoute] int chatId)
        {
            Chat chat = chatPersistence.Chats.Single(x => x.ChatId == chatId);

            return Ok(chat.Messages.Select(x => x.ToDTO()));
        }

        /// <summary>
        /// Add a message to an existing chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("{chatId}/{answer}")]
        public async Task<ActionResult<MessageDTO>> ChooseAnswerForMessageQuestion([FromServices] IHubContext<NotificationHub> hubContext, [FromRoute] int chatId, [FromBody] MessageDTO message, [FromRoute] string answer)
        {
            var question = message.Text;

            Chat chat = chatPersistence.Chats.Single(x => x.ChatId == chatId);

            if (question.ToLower().Contains("Atem".ToLower()) && answer.ToLower() == "ja")
            {
                chat.Messages.Add(new Message
                {
                    MessageType = MessageType.PulseExercise
                });
                chat.Messages.Add(new Message
                {
                    Text = "Hoffentlich konnten sie sich entspannen",
                    MessageType = MessageType.Text
                });
            }
            if (answer.ToLower().Contains("Video".ToLower()))
            {

            }
            if(answer.ToLower() == "nein")
            {
                chat.Messages.Add(new Message
                {
                    Text = "Wie kann ich dir sonst weiterhelfen?",
                    MessageType = MessageType.Text
                });
            }

            await hubContext.Clients.All.SendAsync("Update");

            return Ok();
        }
    }
}
