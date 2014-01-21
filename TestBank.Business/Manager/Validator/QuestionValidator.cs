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
            RuleFor(q => q.Description).NotEmpty().WithMessage("Question Description is required.");
            RuleFor(q => q.Category).NotEmpty().WithMessage("Question Category is required.");
            RuleFor(q => q.CorrectScore)
                .InclusiveBetween(1, 10).WithMessage("'CorrectScore' must be between 1 to 10.");
            RuleFor(q => q.WrongScore)
                .InclusiveBetween(0, 10).WithMessage("'WrongScore' must be between 0 to 10.");
            RuleFor(q => q.Options)
                    .NotEmpty().WithMessage("Atleast 2 options are required for a Question.")
                    .Must(OptionIdMustBeUnique).WithMessage("Option 'Id' must be unique.")
                    .Must(CorrectAnswerCountZero).WithMessage("There must be one correct answer.")
                    .Must(CorrectAnswerCountMoreThanOne).WithMessage("There must be only one correct answer to a question.")
                    .Must(HaveFewerThanTwoOrMoreThanTen).WithMessage("Must have atleast 2 Option's and not more than 10.");
            RuleFor(q => q.Options).SetCollectionValidator(new OptionValidator());
        }

        private bool HaveFewerThanTwoOrMoreThanTen(IList<Option> opts)
        {
            if (opts != null && (opts.Count < 2 || opts.Count > 10))
            {
                return false;
            }
            return true;
        }

        private bool CorrectAnswerCountZero(IList<Option> opts)
        {
            if (opts != null)
            {
                var count = opts.Where(o => o.IsCorrect == true).Count();
                if (count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CorrectAnswerCountMoreThanOne(IList<Option> opts)
        {
            if (opts != null)
            {
                var count = opts.Where(o => o.IsCorrect == true).Count();
                if (count > 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool OptionIdMustBeUnique(IList<Option> opts)
        {
            if (opts != null)
            {
                var duplicateKey = opts.GroupBy(o => o.Id, StringComparer.InvariantCultureIgnoreCase).Where(x => x.Count() > 1).Select(x => x.Key).ToList(); //.ToDictionary(x => x.Key, x => x.Count());
                if (duplicateKey != null && duplicateKey.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public class OptionValidator : AbstractValidator<Option>
        {
            public OptionValidator()
            {
                RuleFor(q => q.Id)
                    .NotEmpty().WithMessage("Option 'Id' Required.")
                    .Matches("^[A-z]{1,3}$").WithMessage("Option 'Id' must be 1 to 3 characters and only alphabets are allowed.");
                ;

                RuleFor(q => q.Description).NotEmpty().WithMessage("Options 'Description' required.");
                RuleFor(q => q.Type).Must(validateType).WithMessage("Option 'Type' is required.");
                //RuleFor(o => o.IsCorrect)
            }

            public bool validateType(OptionType type)
            {
                return (type != 0);
            }
        }
    }
}
