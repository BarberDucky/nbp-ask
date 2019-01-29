using nbp_ask_data.DataProvider;
using nbp_ask_data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nbp_ask_api.Controllers
{
    public class MessageController : ApiController
    {
        // GET: api/Message
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Message/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Message
        public string Post([FromBody]MessageDTO dto)
        {
            return MessageDataProvider.CreateMessage(dto);
        }

        // POST: api/Message
        [HttpPost]
        [Route("api/Message/AddMessageToConversation")]
        public string Post([FromBody]MessageWithConversationDTO dto)
        {
            return MessageDataProvider.AddMessageToConversation(dto);
        }

        // DELETE: api/Message/5
        [HttpDelete]
        [Route("api/Message/{msgId}/Conversation/{convId}")]
        public bool Delete(string msgId, string convId)
        {
            return MessageDataProvider.DeleteMessage(msgId, convId);
        }
    }
}
