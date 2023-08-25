using Microsoft.AspNetCore.Mvc;
using Server.DomainFeatures.ChatAggregate.Domain;
using Server.DomainFeatures.ChatAggregate.Infrastructure;
using Server.Services;
using Shared.Chat;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [HttpGet("{chatId}")]
        public ActionResult<ChatDTO> Get(int chatId)
        {
            return Ok(chatPersistence.Chats.Single(c => c.ChatId == chatId));
        }

        [HttpPost]
        public ActionResult CreateChat(ChatDTO chat)
        {
            chat.Id = randomGenerator.GenerateRandomInt();

            chatPersistence.AddChat(Chat.FromDTO(chat));

            return Ok();
        }

        [HttpPost("messages/{chatId}")]
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
    }
}
