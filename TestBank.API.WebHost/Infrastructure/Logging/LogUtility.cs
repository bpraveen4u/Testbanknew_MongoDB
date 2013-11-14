using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TestBank.API.WebHost.Infrastructure.Logging
{
    public class LogUtility
    {
        public static string BuildExceptionMessage(Exception x)
        {

            Exception logException = x;
            if (x.InnerException != null)
                logException = x.InnerException;

            var strErrorMsg = new StringBuilder();

            // Get the error message
            strErrorMsg.Append("Message :" + logException.Message);

            // Source of the message
            strErrorMsg.Append(" , Source :" + logException.Source);

            // Stack Trace of the error
            if (logException.StackTrace != null)
                strErrorMsg.Append(" , Stack Trace :" + logException.StackTrace.Replace(Environment.NewLine, " , "));

            // Method where the error occurred
            strErrorMsg.Append(" , TargetSite :" + logException.TargetSite);

            return strErrorMsg.ToString();
        }

        public static void ChangeLoggingLevel(LogLevel level)
        {
            LoggingConfiguration config = LogManager.Configuration;
            String loggerNamePattern = config.LoggingRules.SingleOrDefault().LoggerNamePattern;
            config.LoggingRules.Remove(config.LoggingRules.SingleOrDefault());

            LoggingRule rule = new LoggingRule(loggerNamePattern, level, config.ConfiguredNamedTargets.SingleOrDefault());
            //rule.EnableLoggingForLevel(level);
            config.LoggingRules.Add(rule);

            LogManager.ReconfigExistingLoggers();
        }
    }

    public static class LogFactory
    {
        static ILogger _logger;

        public static ILogger Logger
        {
            get
            {
                if (_logger == null) _logger = new NLogLogger();
                return _logger;
            }
        }
    }
}