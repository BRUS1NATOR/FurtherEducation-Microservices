//using System;
//using FluentValidation;
//using Users.API.Keycloak;
//using Users.API.Requests.Users;

//namespace Users.API.Validations
//{
//    public class UserValidator : AbstractValidator<KeycloakUserEntity>
//    {
//        public UserValidator()
//        {
//            RuleFor(e => e.Username)
//                .NotEmpty()
//                .MinimumLength(4)
//                .MaximumLength(16)
//                .Must(x => !x.Equals("admin", StringComparison.OrdinalIgnoreCase))
//                .Matches("^[0-9a-zA-Z]*$");

//            RuleFor(e=>e.Email)
//                .NotEmpty()
//                .EmailAddress()
//                .WithMessage("Email address is not valid");

//            //RuleFor(e => e.Firstname)
//            //    .NotEmpty()
//            //    .MinimumLength(2)
//            //    .MaximumLength(20);

//            //RuleFor(e => e.Lastname)
//            //    .NotEmpty()
//            //    .MinimumLength(2)
//            //    .MaximumLength(20);
//        }
//    }
//}
