using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace FurtherEducation.Common
{
    public class MongoObjectIdAttribute : ValidationAttribute
    {
        public string GetErrorMessage(object value) =>
            $"ObjectId is incorrect - {value}";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (!ObjectId.TryParse(value.ToString(), out _))
            {
                return new ValidationResult(GetErrorMessage(value));
            }

            return ValidationResult.Success;
        }
    }
}