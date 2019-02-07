using nbp_ask_data.DataProvider;
using nbp_ask_data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<ConversationWithMessagesDTO> Post([FromBody]CreateMessageDTO dto)
        {
            var res = await MessageDataProvider.CreateMessage(dto);
            return res;
        }

        // POST: api/Message
        [HttpPost]
        [Route("api/Message/AddMessageToConversation")]
        public async Task<ConversationWithMessagesDTO> Post([FromBody]MessageWithConversationDTO dto)
        {
            var res = await MessageDataProvider.AddMessageToConversation(dto);
            return res;
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
