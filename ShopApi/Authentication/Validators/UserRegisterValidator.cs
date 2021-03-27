using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ShopApi.Dto;

namespace ShopApi.Authentication.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format");


            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2);

            RuleFor(x => x.LastName).NotEmpty().MinimumLength(2);

            RuleFor(x => x.PhoneNumber).NotEmpty().Matches("^(\\+4|)?(07[0-8]{1}[0-9]{1}|02[0-9]{2}|03[0-9]{2}){1}?(\\s|\\.|\\-)?([0-9]{3}(\\s|\\.|\\-|)){2}$");

            RuleFor(x => x.Password).NotEmpty()
                                                          .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")
                                                          .WithMessage("Password must contains minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character");


        }
    }
}
