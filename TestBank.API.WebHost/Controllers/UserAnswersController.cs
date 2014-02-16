using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using NLog;
using TestBank.Business.Core;
using System.IO;
using System.Net.Http.Headers;
using TestBank.Business.Manager;
using TestBank.Entity;

namespace TestBank.Business.Controllers
{
    public class UserAnswersController : ApiController
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UserAnswersManager manager;

        public UserAnswersController(UserAnswersManager manager)
        {
            this.manager = manager;
            logger.Debug("Controller constructor.");
        }

        public IEnumerable<UserAnswer> GetAll(int testId)
        {
            return manager.GetAll(testId);
        }

        public UserAnswer Get(int id)
        {
            UserAnswer answer = manager.GetUserAnswer(id);

            if (answer != null)
            {
                return answer;
            }
            else
            {
                var message = string.Format("Answer with id = '{0}' not found", id);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
        }

        public HttpResponseMessage Post(UserAnswer answers)
        {
            if (answers == null)
            {
                var message = string.Format("Request data not in correct format");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            answers.TotalQuestions = 0;
            answers = manager.AddAnswer(answers);
            var response = Request.CreateResponse<UserAnswer>(HttpStatusCode.Created, answers);

            string uri = Url.Link("useranswers", new { id = answers.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        
        public HttpResponseMessage Put(int id, UserAnswer answers)
        {
            if (answers == null)
            {
                var message = string.Format("Request data not in correct format");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            answers.Id = id;
            answers = manager.UpdateAnswer(answers);
            
            var response = Request.CreateResponse<UserAnswer>(HttpStatusCode.OK, answers);
            return response;
        }

        //api/UserAnswers/Export?testId=9&output=Excel
        [HttpGet]
        [ActionName("Export")]
        public HttpResponseMessage Export(int testId, string output)
        {
            if (string.Equals(output, "Excel", StringComparison.InvariantCultureIgnoreCase))
            {
                var excelData = manager.ExportToExcel(testId);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new MemoryStream(excelData);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Data.xls"
                };

                return result;
            }
            return Request.CreateResponse<string>(HttpStatusCode.NotFound, "Resource Not Fournd!");
        }
    }
}
