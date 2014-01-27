using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace TestBank.Web.ViewModels.Validations
{
    public class AssessmentViewModelValidator : AbstractValidator<AssessmentViewModel>
    {
        public AssessmentViewModelValidator()
        {
            RuleFor(a => a.TestName).NotEmpty().WithMessage("Test Name required.");
            RuleFor(a => a.QuestionIds).NotNull().WithMessage("Select question to the test.");
            RuleFor(a => a.Duration).InclusiveBetween(5, 60).WithMessage("duration must be in between 5 and 60 minutes");
            RuleFor(a => a.MaxOptions).GreaterThan(1).WithMessage("Maximum Options must be greater than 1.");
        }
    }
}