using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.Model;

namespace nbp_ask_data.DTOs
{
    public class CreateConversationDTO
    {
        public String UserId1 { get; set; }
        public String UserId2 { get; set; }

        public static Conversation FromDTO(CreateConversationDTO dto)
        {
            return new Conversation()
            {
                UserId1 = dto.UserId1,
                UserId2 = dto.UserId2
            };
        }
    }
}
