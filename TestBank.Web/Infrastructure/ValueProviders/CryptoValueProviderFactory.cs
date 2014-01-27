using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestBank.Web.Infrastructure.Encryption;
using System.Globalization;
using System.Web.Routing;
using System.Collections.Specialized;

namespace TestBank.Web.Infrastructure.ValueProviders
{
    public class CryptoValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new CryptoValueProvider(controllerContext.HttpContext.Request.QueryString);
        }
    }

    public class CryptoValueProvider : IValueProvider
    {
        NameValueCollection queryStrings = null;

        public CryptoValueProvider(NameValueCollection queryString)
        {
            this.queryStrings = queryString;
        }

        public bool ContainsPrefix(string prefix)
        {
            if (prefix == "testId" || prefix == "responseId")
            {
                return true;
            }
            return false;
        }

        public ValueProviderResult GetValue(string key)
        {
            if (!ContainsPrefix(key))
                return null;
            
            var val = EncryptDecryptQueryString.Decrypt(queryStrings[key]);
            ValueProviderResult result = new ValueProviderResult(val, val, CultureInfo.CurrentCulture);
            return result;
        }
    }
}