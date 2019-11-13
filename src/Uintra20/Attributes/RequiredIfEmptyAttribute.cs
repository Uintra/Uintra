using System.ComponentModel.DataAnnotations;

namespace Uintra20.Attributes
{
    public class RequiredIfEmptyAttribute : ValidationAttribute
    {
        public string OtherProperty { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (IsPropertyHasValue(value) || IsPropertyHasValue(GetOtherPropertyValue(validationContext)))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Other property required a value if current property is empty");
        }

        protected object GetOtherPropertyValue(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
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