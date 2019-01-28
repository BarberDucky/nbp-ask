using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.Model
{
    public class Question
    {
        public String Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public int Points { get; set; }
        public bool IsAnswered { get; set; }
        public List<String> Tags { get; set; }
        public String PosterId { get; set; }
        public List<Answer> Answers { get; set; }


        public Question()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
