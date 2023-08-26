using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DomainFeatures.ChatAggregate.Domain;
using Server.DomainFeatures.ChatAggregate.Infrastructure;
using Server.Services;
using Shared.Chat;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<ChatDTO> CreateChat(ChatDTO chatDTO)
        {
            chatDTO.Id = randomGenerator.GenerateRandomInt();

            var chat = Chat.FromDTO(chatDTO);
            chatPersistence.AddChat(chat);

            var message = chat.Messages.Last();
            if (botService.CheckIfMessageIsBotHandable(message))
            {
                botService.AnswerToMessageAsync(chat, message);
            }

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
        public ActionResult CreateMessageInChat([FromRoute] int chatId, MessageDTO messageDTO)
        {
            Chat chat = chatPersistence.Chats.Single(x => x.ChatId == chatId);
            Message message = Message.FromDTO(messageDTO);

            chat.AddMessage(message);

            if (botService.CheckIfMessageIsBotHandable(message))
            {
                botService.AnswerToMessageAsync(chat, message);
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
        [HttpPost("{chatId}/{question}/{answer}")]
        public ActionResult<MessageDTO> ChooseAnswerForMessageQuestion([FromRoute] int chatId, [FromRoute] string question, [FromRoute] string answer)
        {
            Chat chat = chatPersistence.Chats.Single(x => x.ChatId == chatId);

            

            return Ok();
        }
    }
}
