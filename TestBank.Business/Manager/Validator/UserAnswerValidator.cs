using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using TestBank.Entity;

namespace TestBank.Business.Core.Validators
{
    public class UserAnswerValidator : AbstractValidator<UserAnswer>
    {
        public UserAnswerValidator()
        {
            RuleFor(u => u.AssessmentId)
                .NotEmpty().WithMessage("'AssessmentId' is required.")
                //.Matches(@"[1-9][0-9]*").WithMessage("'AssessmentId' must be positive number.")
                ;
            //RuleFor(u => u.UserId)
            //    .NotEmpty().WithMessage("'UserId' is Required.")
            //    .Matches(@"[1-9][0-9]*").WithMessage("'UserId' must be positive number.")
            //    ;
            RuleFor(u => u.Answers).SetCollectionValidator(new AnswersValidator());
        }

        public class AnswersValidator : AbstractValidator<UserAnswer.Answer>
        {
            public AnswersValidator()
            {
                RuleFor(a => a.Question)
                    .NotEmpty().WithMessage("'Question' is required.")
                    ;
            }
        }
    }
}
