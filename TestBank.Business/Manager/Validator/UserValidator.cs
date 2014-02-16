using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using TestBank.Entity;

namespace TestBank.Business.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("'Email' is required.")
                .EmailAddress().WithMessage("'Email' not in the correct format")
                ;
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("'FirstName' is Required.")
                .Length(2, 120).WithMessage("'FirstName' must be between 2 to 120 characters.")
                ;
            RuleFor(u => u.Id)
                .NotEmpty().WithMessage("'User Id' is required.")
                .Length(6, 15).WithMessage("'UserId' must be between 6 to 15 charachers.")
                .Matches("^(?=.*[a-zA-Z])[^\\*\\s]{6,15}$").WithMessage("Allowed characters for 'UserId' are alphanumeric & special characters.");
                ;
        }
    }
}
