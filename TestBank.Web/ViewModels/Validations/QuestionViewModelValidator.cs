using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
//using FluentValidation.Mvc;

namespace TestBank.Web.ViewModels.Validations
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(q => q.Description).NotEmpty().WithMessage("Question description required.");
            RuleFor(q => q.Options)
                .NotNull().WithMessage("At least 2 options required")
                .NotEmpty().WithMessage("At least 2 options required")
                .Must(CorrectAnswerCountZero).WithMessage("Please select one correct answer.")
                .Must(CorrectAnswerCountMoreThanOne).WithMessage("Please choose only one correct answer.")
                .Must(HaveFewerThanTwoOrMoreThanTen).WithMessage("Must have atleast 2 Options or not more than 10.");
            //RuleFor(q => q.Options).SetValidator(new OptionCountValidator<QuestionViewModel.OptionViewModel>()).WithMessage("opt count > 2");
            RuleFor(q => q.Options).SetCollectionValidator(new OptionViewModelValidator());
        }

        private bool HaveFewerThanTwoOrMoreThanTen(IList<QuestionViewModel.OptionViewModel> opts)
        {
            if (opts != null && (opts.Count < 2 || opts.Count > 10))
            {
                return false;
            }
            return true;
        }

        private bool CorrectAnswerCountZero(IList<QuestionViewModel.OptionViewModel> opts)
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

        private bool CorrectAnswerCountMoreThanOne(IList<QuestionViewModel.OptionViewModel> opts)
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
    }

    public class OptionViewModelValidator : AbstractValidator<QuestionViewModel.OptionViewModel>
    {
        public OptionViewModelValidator()
        {
            RuleFor(q => q.Text).NotEmpty().WithMessage("Option required.");
            //RuleFor(o => o.IsCorrect)
        }
    }
}