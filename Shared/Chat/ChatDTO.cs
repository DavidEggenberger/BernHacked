using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Chat
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public List<MessageDTO> Messages { get; set; }
        public ChatDTO()
        {
            Messages = new List<MessageDTO>()
            {
                new MessageDTO
                {
                    Text = "Wilkommen bei uns",
                    MessageSenderType = MessageSenderTypeDTO.DargeboteneHandBot
                }
            };
        }
    }
}
