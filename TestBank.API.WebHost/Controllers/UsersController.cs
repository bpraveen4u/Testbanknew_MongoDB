using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using NLog;
using System.Net;
using System.Net.Http;
using TestBank.Business.Manager;
using TestBank.Entity;

namespace TestBank.Business.Controllers
{
    public class UsersController : ApiController
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UsersManager manager;

        public UsersController(UsersManager manager)
        {
            this.manager = manager;
            logger.Debug("User Controller created");
        }

        public IEnumerable<User> GetAll()
        {
            return manager.GetAll();
        }

        public User Get(string id)
        {
            User user = manager.Get(id);
            
            if (user != null)
            {
                return user;
            }
            else
            {
                var message = string.Format("User with id = '{0}' not found", id);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post(User user)
        {
            if (user == null)
            {
                var message = string.Format("Request data not in correct format");
                HttpError err = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, err);
            }
            user = manager.Post(user);
            var response = Request.CreateResponse<User>(HttpStatusCode.Created, user);

            string uri = Url.Link("Users", new { id = user.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public HttpResponseMessage Put(int id, User user)
        {
            if (user == null)
            {
                var message = string.Format("Request data not in correct format");
                HttpError err = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, err);
            }
            user.Id = id.ToString();
            var userUpdated = manager.Update(user);
            var response = Request.CreateResponse<User>(HttpStatusCode.OK, userUpdated);
            return response;
        }

    }
}
