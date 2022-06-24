using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FurtherEducation.Common.Exceptions
{
    public class ValidationExceptionMessage : EduExceptionMessage
    {
        public ValidationExceptionMessage() : base()
        {

        }

        public ValidationExceptionMessage(string message, int code = StatusCodes.Status400BadRequest) : base(message, code)
        {

        }

        public static ValidationExceptionMessage CreateExceptionMessage(ValidationResult validationException)
        {
            return new ValidationExceptionMessage("Данные не удовлетворяют бизнес требованиям!\n" + string.Join("\n", validationException.Errors.Select(x => x.ErrorMessage)));
        }
    }
}