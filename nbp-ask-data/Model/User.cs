using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.Model
{
    public class User
    {
        public String Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public List<String> Questions { get; set; }
        public List<UserConversation> UserConversaions { get; set; }
    }
}
