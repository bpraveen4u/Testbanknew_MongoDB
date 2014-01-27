using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using TestBank.Web.Infrastructure.ServiceProxy;
using System.Threading;
using TestBank.Entity.Errors;
using PagedList;
using System.Configuration;
using RestSharp.Serializers;
using RestSharp.Deserializers;
using Newtonsoft.Json;
using System.IO;
using TestBank.Entity.Models;

namespace TestBank.Web.Infrastructure.ServiceProxy
{
    public static class ResourceEndPoint
    {
        public static string Question_All = "/Questions";
        //public static string Question_All_Count = "/Questions?countOnly={countOnly}";
        public static string Question_Get = "/Questions/{id}";
        public static string Question_Post = "/Questions";
        public static string Question_Put = "/Questions/{id}";

        public static string Assessments_All = "/Assessments";
        public static string Assessments_Get = "/Assessments/{id}";
        public static string Assessments_Post = "/Assessments";
        public static string Assessments_Put = "/Assessments/{id}";

        public static string UserAnswers_All = "/UserAnswers?testId={testId}";
        public static string UserAnswers_Post = "/UserAnswers";
        public static string UserAnswers_Put = "/UserAnswers/{id}";
        public static string UserAnswers_Get = "/UserAnswers/{id}";

        public static string User_Get = "/Users/{id}";
        //public static string User_Get_UserId = "/Users?userId={userId}";
        public static string User_Post = "/Users";
        
    }

    public class TestBankApiProxy
    {
        
        public TestBankApiProxy()
        {

        }

        public static string Authenticate(Credentials credentials)
        {
            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(null, "/auth", Method.POST, null);

            if (credentials != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(credentials);
            }

            var response = client.Execute<Guid>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data.ToString();
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    throw new ApiError(response.StatusDescription, errors.ToList());
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        private static RestClient CreateHttpClient()
        {
            var client = new RestClient(ConfigurationManager.AppSettings.Get("ApiBaseUrl"))
            {
                Authenticator = new HttpBasicAuthenticator("Praveen", "Password")
            };

            client.RemoveHandler("application/json");
            client.AddHandler("application/json", new JsonDotNetSerializer());
            
            return client;
        }

        private static RestRequest CreateHttpRequest(string apiKey, string apiEndPoint, Method method, List<Tuple<string,string>> urlSegments)
        {
            var request = new RestRequest(apiEndPoint, method);
            if (urlSegments != null && urlSegments.Count > 0)
            {
                foreach (var item in urlSegments)
                {
                    request.AddUrlSegment(item.Item1, item.Item2);
                }
            }
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            if(apiKey != null)
                request.AddHeader("APIKey", apiKey);
            //request.JsonSerializer = new JsonDotNetSerializer();

            return request;
        }

        public static T Get<T>(string apiKey, string apiEndPoint) where T : new()
        {
            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey, apiEndPoint, Method.GET, null);

            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    //var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    //var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    //throw new ApiError(response.StatusDescription, errors.ToList());
                    throw new ApiError(response.ErrorMessage);
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        //public static void BeforeDeserialize(IRestResponse response)
        //{
        //    //response.Request.JsonSerializer = 
        //}

        public static T Get<T>(string apiKey, string apiEndPoint, List<Tuple<string, string>> urlSegments) where T : new()
        {
            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey, apiEndPoint, Method.GET, urlSegments);

            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new ApiError("Unauthorized. You do not have access to the requested resource.");
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    //var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    //var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    //throw new ApiError(response.StatusDescription, errors.ToList());

                    if (response.ErrorMessage == null)
                    {
                        throw new ApiError(response.Content);
                    }
                    throw new ApiError(response.ErrorMessage);
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        public static T Get<T>(string apiKey, string apiEndPoint, int id) where T : new()
        {
            return Get<T>(apiKey, apiEndPoint, id.ToString());
        }

        public static T Get<T>(string apiKey, string apiEndPoint, string id) where T : new()
        {
            var urlSegments = new List<Tuple<string, string>>();
            urlSegments.Add(new Tuple<string, string>("id", id));

            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey, apiEndPoint, Method.GET, urlSegments);

            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    //var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    //var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    //throw new ApiError(response.StatusDescription, errors.ToList());
                    throw new ApiError(response.ErrorMessage);
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        public static T Post<T>(string apiKey, object objectToPost, string apiEndPoint) where T : new()
        {
            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey,apiEndPoint, Method.POST, null);
            
            if (objectToPost != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(objectToPost);
            }
            
            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    throw new ApiError(response.StatusDescription, errors.ToList());
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        public static T Put<T>(string apiKey, object objectToPost, string apiEndPoint, int id) where T : new()
        {
            var urlSegments = new List<Tuple<string, string>>();
            urlSegments.Add(new Tuple<string, string>("id", id.ToString()));

            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey,apiEndPoint, Method.PUT, urlSegments);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(objectToPost);
            
            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    throw new ApiError(response.StatusDescription, errors.ToList());
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        public static T Delete<T>(string apiKey, string apiEndPoint, int id) where T : new()
        {
            var urlSegments = new List<Tuple<string, string>>();
            urlSegments.Add(new Tuple<string, string>("id", id.ToString()));

            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey, apiEndPoint, Method.DELETE, urlSegments);

            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    //var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
                    //var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<String>>(response.Content, enumConverter);
                    //throw new ApiError(response.StatusDescription, errors.ToList());
                    throw new ApiError(response.ErrorMessage);
                }
                else if (response.StatusCode == 0) // while the input params passed as null
                {
                    throw new ApiError(response.ErrorMessage);
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new ApiError(response.StatusDescription);
                }
                else
                {
                    throw new ApiError(response.StatusDescription, new List<string>() { response.Content });
                }
            }
        }

        public static byte[] DownloadFile(string apiKey, string apiEndPoint, List<Tuple<string, string>> urlSegments)
        {
            RestClient client = CreateHttpClient();
            RestRequest request = CreateHttpRequest(apiKey,apiEndPoint, Method.GET, urlSegments);

            var responseData = client.DownloadData(request);

            return responseData;
        }



    }

    public class JsonDotNetSerializer : ISerializer, IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        /// <summary>
        /// Default serializer
        /// </summary>
        public JsonDotNetSerializer()
        {
            ContentType = "application/json";

            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        /// <summary>
        /// Default serializer with overload for allowing custom Json.NET settings
        /// </summary>
        public JsonDotNetSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = "application/json";
            _serializer = serializer;
        }

        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    _serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            try
            {
                var deserializedObject = JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings()
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    Culture = Thread.CurrentThread.CurrentCulture
                });

                return deserializedObject;
            }
            catch (Exception)
            {
                return default(T);
            }
            
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }
    }
}