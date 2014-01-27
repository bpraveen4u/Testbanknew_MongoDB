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
                name: "AssessmentQuestionOptions",
                routeTemplate: "api/assessments/{assessmentId}/questions/{questionId}/options/{optionId}",
                defaults: new { controller = "assessments", action = "GetAssessmentQuestionOptions", optionId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "AssessmentQuestions",
                routeTemplate: "api/assessments/{assessmentId}/questions/{questionId}",
                defaults: new { controller = "assessments", action = "GetAssessmentQuestions", questionId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "QuestionOptions",
                routeTemplate: "api/questions/{questionId}/options/{optionId}",
                defaults: new { controller = "questions", action = "GetQuestionOptions", optionId = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "Category",
            //    routeTemplate: "api/questions/category/",
            //    defaults: new { controller = "questions", action = "GetCategory" /*, categoryName = RouteParameter.Optional */}
            //);

            config.Routes.MapHttpRoute(
                name: "QuestionCategory",
                routeTemplate: "api/questions/category/{categoryName}",
                defaults: new { controller = "questions", action = "GetAllByCategory" /*, categoryName = RouteParameter.Optional */}
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
            jsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //json.SerializerSettings..PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(new CustomXmlFormatter());
        }
    }
}
