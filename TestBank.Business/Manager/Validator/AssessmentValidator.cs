using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using TestBank.Entity;

namespace TestBank.Business.Manager.Validator
{
    public class AssessmentValidator : AbstractValidator<Assessment>
    {
        public AssessmentValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("Test name is required.");

            RuleFor(v => v.Duration).GreaterThan(0).WithMessage("Duration must be greater than zero.");
        }
    }
}
