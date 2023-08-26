using Shared.Chat;
using System.Collections.Generic;

namespace Server.DomainFeatures.ChatAggregate.Domain
{
    public class Message
    {
        public string Text { get; set; }
        public string Base64Data { get; set; }
        public bool Bot { get; set; }
        public MessageType MessageType { get; set; }
        public MessageSenderType MessageSenderType { get; set; }
        public List<string> Answers { get; set; }

        public static Message FromDTO(MessageDTO messageDTO)
        {
            return new Message
            {
                Text = messageDTO.Text,
                MessageSenderType = (MessageSenderType)messageDTO.MessageSenderType,
                MessageType = (MessageType)messageDTO.MessageType,
                Base64Data = messageDTO.Base64Data,
                Bot = messageDTO.Bot,
            };
        }

        public MessageDTO ToDTO()
        {
            return new MessageDTO()
            {
                Text = this.Text,
                MessageSenderType = (MessageSenderTypeDTO)this.MessageSenderType,
                MessageType = (MessageTypeDTO)this.MessageType,
                Base64Data = this.Base64Data,
                Answers = this.Answers,
                Bot = this.Bot,
            };
        }
    }
}
