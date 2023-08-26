using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Chat
{
    public class MessageDTO
    {
        public DateTime SentAt { get; set; }
        public string Text { get; set; }
        public bool Bot { get; set; }
        public MessageTypeDTO MessageType { get; set; }
        public MessageSenderTypeDTO MessageSenderType { get; set; }
        public string Base64Data { get; set; }
        public List<string> Answers { get; set; }
    }
}
