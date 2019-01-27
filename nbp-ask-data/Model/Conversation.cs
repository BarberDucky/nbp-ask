﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.Model
{
    public class Conversation
    {
        public String Id { get; set; }
        public String UserId1 { get; set; }
        public String UserId2 { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
