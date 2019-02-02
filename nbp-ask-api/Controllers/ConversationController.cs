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
    public class ConversationController : ApiController
    {
        // GET: api/Conversation
        [HttpGet]
        [Route("api/Conversation/{convId}/User/{userId}")]
        public ConversationWithMessagesDTO Get(string convId, string userId)
        {
            return ConversationDataProvider.ReadConversation(convId, userId);
        }

        // GET: api/Conversation/5
        [HttpGet]
        [Route("api/Conversation/UserConversations/{id}")]
        public List<ReadConversationDTO> Get(string id)
        {
            return ConversationDataProvider.GetConversationsWithUser(id);
        }

        // DELETE: api/Conversation/5
        public bool Delete(string id)
        {
            return ConversationDataProvider.DeleteConversation(id);
        }
    }
}
