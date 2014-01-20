using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TestBank.API.WebHost.Formatters;
using TestBank.API.WebHost.Infrastructure.AutoMapper;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using TestBank.API.WebHost.Infrastructure.Converters;

namespace TestBank.API.WebHost
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "AssessmentQuestions",
                routeTemplate: "api/assessments/{assessmentId}/questions/{id}",
                defaults: new { controller = "assessments", action = "GetAssessmentQuestions", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Assessments",
                routeTemplate: "api/assessments/{id}",
                defaults: new { controller = "assessments", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Questions",
                routeTemplate: "api/questions/{id}",
                defaults: new { controller = "questions", id = RouteParameter.Optional }
            );

            AutoMapperConfiguration.Configure();

            //JSON serialization settings
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.Converters.Add(new LinkModelConverter());
            //json.SerializerSettings..PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(new CustomXmlFormatter());
        }
    }
}
