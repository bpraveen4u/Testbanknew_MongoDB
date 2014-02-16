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
            RouteConfig.Register(config);

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
