using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.Model
{
    public class Message
    {
        public String Id { get; set; }
        public String Content { get; set; }    
        public DateTime Timestamp { get; set; }
        public String SenderId { get; set; }
        public String SenderUsername { get; set; }

        public Message()
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
        }
    }
}
