using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TestBank.API.WebHost
{
    public static class RouteConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Routes.MapHttpRoute(
                name: "auth",
                routeTemplate: "api/auth/",
                defaults: new { controller = "auth", action="login"}
            );

            config.Routes.MapHttpRoute(
                name: "useranswers",
                routeTemplate: "api/userAnswers/{id}",
                defaults: new { controller = "userAnswers", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Users",
                routeTemplate: "api/users/{id}",
                defaults: new { controller = "users", id = RouteParameter.Optional }
            );

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
        }
    }
}