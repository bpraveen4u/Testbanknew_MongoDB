using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Validators;

namespace TestBank.Web.ViewModels.Validations
{
    public class OptionCountValidator<T> : PropertyValidator {

        public OptionCountValidator() 
		    : base("Property {PropertyName} should contain atlease 2 and not more than 10.") {
		
	    }

	    protected override bool IsValid(PropertyValidatorContext context) {
		    var list = context.PropertyValue as IList<T>;

		    if(list != null && (list.Count < 2 || list.Count >= 10)) {
			    return false;
		    }

		    return true;
	    }
    }
}