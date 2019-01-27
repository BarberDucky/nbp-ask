using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.Model
{
    public class Answer
    {
        public String Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Content { get; set; }
        public int Points { get; set; }
        public bool IsTrue { get; set; }
        public User Poster { get; set; }

        public Answer()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
