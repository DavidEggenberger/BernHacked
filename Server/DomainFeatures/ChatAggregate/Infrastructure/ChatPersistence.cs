using Server.DomainFeatures.ChatAggregate.Domain;
using Shared.Chat;
using System.Collections;
using System.Collections.Generic;

namespace Server.DomainFeatures.ChatAggregate.Infrastructure
{
    public class ChatPersistence
    {
        public IList<Chat> Chats { get; set; }
        public ChatPersistence()
        {
            Chats = new List<Chat>()
            {
                new Chat
                {
                    ChatId = 1,
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Text = "demo message"
                        }
                    }
                }
            };
        }

        public void AddChat(Chat chat)
        {
            Chats.Add(chat);
        }

        
    }
}
