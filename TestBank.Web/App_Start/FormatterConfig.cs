using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace TestBank.Web
{
    public static class FormatterConfig
    {
        public static void RegisterGlobalFormatters(MediaTypeFormatterCollection formatters)
        {
            var jsonSerializerSettings = formatters.JsonFormatter.SerializerSettings;
            //jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //var jsonFormatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            

            // At index 0 so that it will try this before the default handler.
            //formatters.Insert(0, new ChangeSetJsonFormatter(jsonSerializerSettings));

            formatters.Remove(formatters.XmlFormatter);
        }
    }
}