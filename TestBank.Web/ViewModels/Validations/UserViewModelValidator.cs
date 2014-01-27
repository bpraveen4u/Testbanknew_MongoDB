using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
//using FluentValidation.Mvc;

namespace TestBank.Web.ViewModels.Validations
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(u => u.Email)
               .NotEmpty().WithMessage("'Email' is required.")
               .EmailAddress().WithMessage("'Email' not in the correct format")
               ;
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("'FirstName' is Required.")
                .Length(2, 120).WithMessage("'FirstName' must be between 2 to 120 characters.")
                ;
            RuleFor(u => u.UserId)
                .NotEmpty().WithMessage("'User Id' is required.")
                .Length(6, 15).WithMessage("'UserId' must be between 6 to 15 charachers.")
                .Matches("^(?=.*[a-zA-Z])[^\\*\\s]{6,15}$").WithMessage("Allowed characters for 'UserId' are alphanumeric & special characters.");
            ;
        }
    }
}