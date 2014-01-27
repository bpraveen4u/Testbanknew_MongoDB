using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBank.Web.Infrastructure.ModelBinder
{
    /*public class CrptoValueProviderDefaultModelBinder : DefaultModelBinder
    {
        private Type _ValueProviderType;

        public CrptoValueProviderDefaultModelBinder(Type valueProviderType)
        {
            if (valueProviderType.GetInterface(typeof(IValueProvider).Name) == null)
                throw new ArgumentException("Type " + valueProviderType + " must implement interface IValueProvider.", "valueProviderType");

            _ValueProviderType = valueProviderType;
        }

        /// <summary>
        /// Before binding the model, set the IValueProvider it uses
        /// </summary>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ValueProvider = GetValueProvider();

            return base.BindModel(controllerContext, bindingContext);
        }

        private IValueProvider GetValueProvider()
        {
            return (IValueProvider)Activator.CreateInstance(_ValueProviderType);
        }
    }*/
}