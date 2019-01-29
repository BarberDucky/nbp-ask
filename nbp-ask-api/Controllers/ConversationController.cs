using nbp_ask_data.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nbp_ask_api.Controllers
{
    public class ConversationController : ApiController
    {
        // GET: api/Conversation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Conversation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Conversation
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Conversation/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Conversation/5
        public bool Delete(string id)
        {
            return ConversationDataProvider.DeleteConversation(id);
        }
    }
}
