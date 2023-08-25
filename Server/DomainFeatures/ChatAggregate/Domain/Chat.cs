using Shared.Chat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.DomainFeatures.ChatAggregate.Domain
{
    public class Chat
    {
        public int ChatId { get; set; }
        public IList<ChatOption> ChatOptions { get; set; }
        public List<Message> Messages { get; private set; }


        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        public static Chat FromDTO(ChatDTO chatDTO)
        {
            return new Chat
            {
                ChatId = chatDTO.Id,
                Messages = chatDTO.Messages.Select(x => Message.FromDTO(x)).ToList(),
            };
        }

        public ChatDTO ToDTO()
        {
            return new ChatDTO
            {
                Id = this.ChatId,
                Messages = Messages.Select(x => x.ToDTO()).ToList()
            };
        }
    }
}
