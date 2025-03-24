using System.ComponentModel.DataAnnotations;

namespace BusTicketCase.MVC.Validation
{
    public class NotEqualAttribute : ValidationAttribute
    {
        private readonly string otherProperty;

        public NotEqualAttribute(string otherProperty)
        {
            this.otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
            if (propertyInfo == null)
            {
                return new ValidationResult($"Property with name {otherProperty} not found.");
            }

            var otherValue = propertyInfo.GetValue(validationContext.ObjectInstance);

            if (value == null && otherValue == null)
            {
                return ValidationResult.Success;
            }

            if (!value.Equals(otherValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "The values must not be equal.");
        }
    }
}
