using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nbp_ask_data.Model;
using nbp_ask_data.DataProvider;
using nbp_ask_data.DTOs;

namespace nbp_ask_api.Controllers
{
    public class UsersController : ApiController
    {
        // GET: api/Users
        public IEnumerable<UserDTO> Get()
        {
            return UserDataProvider.ReadAllUsers();
        }

        // GET: api/Users/5
        public UserDTO Get(String id)
        {
            return UserDataProvider.ReadUser(id);
        }

        [HttpPost]
        [Route("api/Users/Login")]
        public UserDTO Login([FromBody]UserDTO userDTO)
        {
            return UserDataProvider.LoginUser(userDTO);
        }

        // POST: api/Users
        public String Post([FromBody]UserDTO userDTO)
        {
            return UserDataProvider.CreateUser(userDTO);
        }

        // PUT: api/Users/5
        public UserDTO Put(String id, [FromBody]UserDTO userDTO)
        {
            return UserDataProvider.UpdateUser(userDTO, id);
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
        }
    }
}
