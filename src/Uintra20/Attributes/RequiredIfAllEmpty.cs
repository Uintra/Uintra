using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Uintra20.Attributes
{
    public class RequiredIfAllEmptyAttribute : ValidationAttribute
    {
        public string[] DependancyProperties { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (IsPropertyHasValue(value) || DependancyProperties.Any(p => IsPropertyHasValue(GetOtherPropertyValue(validationContext, p))))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Other property required a value if current property is empty");
        }
        protected object GetOtherPropertyValue(ValidationContext validationContext, string propertyName)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(propertyName);
            var value = propertyInfo?.GetValue(validationContext.ObjectInstance, index: null);

            return value;
        }

        private bool IsPropertyHasValue(object value)
        {
            var propertyValue = value as string;
            var isPropertyEmpty = string.IsNullOrWhiteSpace(propertyValue);
            return !isPropertyEmpty;
        }
    }
}