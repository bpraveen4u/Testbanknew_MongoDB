using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBank.Web.Infrastructure.ModelBinder
{
    //public class SetCryptoValueProviderAttribute
    /*[AttributeUsage(AttributeTargets.Class | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class SetCryptoValueProviderAttribute : CustomModelBinderAttribute
    {
        // Originally, this was an action filter, that OnActionExecuting, set the controller's IValueProvider, expecting it to be picked up by the default model binder
        // when binding the model.  Unfortunately, OnActionExecuting occurs AFTER the IValueProvider is set on the DefaultModelBinder.  The only way around this is
        // to create a custom model binder that inherits from DefaultModelBinder, and in its BindModel method set the ValueProvider and then do the standard model binding.

        public SetCryptoValueProviderAttribute(Type valueProviderType)
        {
            if (valueProviderType.GetInterface(typeof(IValueProvider).Name) == null)
                throw new ArgumentException("Type " + valueProviderType + " must implement interface IValueProvider.", "valueProviderType");

            _ValueProviderType = valueProviderType;
        }

        private Type _ValueProviderType;

        public override IModelBinder GetBinder()
        {
            var modelBinder = new CrptoValueProviderDefaultModelBinder(_ValueProviderType);
            return modelBinder;
        }

    }*/
}