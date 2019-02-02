using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.Model
{
    public class Conversation
    {
        public String Id { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime Timestamp { get; set; }
        public String UserId1 { get; set; }
        public String User1Username { get; set; }
        public String UserId2 { get; set; }
        public String User2Username { get; set; }

        public Conversation()
        {
            Messages = new List<Message>();
            Timestamp = DateTime.Now.ToUniversalTime();
        }

        public Conversation(Conversation c)
        {
            this.Id = c.Id;
            this.Timestamp = c.Timestamp;
            this.UserId1 = c.UserId1;
            this.UserId2 = c.UserId2;
            this.User1Username = c.User1Username;
            this.User2Username = c.User2Username;
            Messages = new List<Message>();
        }
    }

}
