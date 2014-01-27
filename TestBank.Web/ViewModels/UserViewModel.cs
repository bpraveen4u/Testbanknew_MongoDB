using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;
using TestBank.Web.ViewModels.Validations;

namespace TestBank.Web.ViewModels
{
    [Validator(typeof(UserViewModelValidator))]
    public class UserViewModel
    {
        public string Id { get; set; }
        public int Sort { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Qualification { get; set; }
        //public Roles Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
    }
}