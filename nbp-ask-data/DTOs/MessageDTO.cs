using nbp_ask_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.DTOs
{
    public class MessageDTO
    {
        public String Content { get; set; }
        public String SenderId { get; set; }
        public String ReceiverId { get; set; }

        public static Message FromDTO(MessageDTO dto)
        {
            return new Message()
            {
                Content = dto.Content,
                SenderId = dto.SenderId
            };
        }
    }

    public class MessageWithConversationDTO
    {
        public String Content { get; set; }
        public String SenderId { get; set; }
        public String ReceiverId { get; set; }
        public String ConversationId { get; set; }

        public static Message FromDTO(MessageWithConversationDTO dto)
        {
            return new Message()
            {
                Content = dto.Content,
                SenderId = dto.SenderId
            };
        }
    }
}
