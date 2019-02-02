using nbp_ask_data.DataProvider;
using nbp_ask_data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nbp_ask_api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MessageController : ApiController
    {

        // GET: api/Message/5
        [HttpGet]
        [Route("api/Message/{msgId}/FromConversation/{convId}")]
        public ReadMessageDTO Get(string msgId, string convId)
        {
            return MessageDataProvider.ReadMessage(convId, msgId);
        }

        // POST: api/Message
        public ConversationWithMessagesDTO Post([FromBody]CreateMessageDTO dto)
        {
            return MessageDataProvider.CreateMessage(dto);
        }

        // POST: api/Message
        [HttpPost]
        [Route("api/Message/AddMessageToConversation")]
        public ConversationWithMessagesDTO Post([FromBody]MessageWithConversationDTO dto)
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
