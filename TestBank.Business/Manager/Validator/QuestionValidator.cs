using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using FluentValidation;

namespace TestBank.Business.Manager.Validator
{
    public class QuestionValidator : AbstractValidator<Question>
    {
        public QuestionValidator()
        {
            RuleFor(q => q.Description).NotEmpty().WithMessage("Question Name is required.");
            RuleFor(q => q.Category).NotEmpty().WithMessage("Question Category is required.");
        }
    }
}
