using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra20.Attributes
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        public GreaterThanAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; set; }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) < 0)
                {
                    return new ValidationResult("EndDate must be greater than StartDate");
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var secondValue = propertyInfo?.GetValue(validationContext.ObjectInstance, null);
            return secondValue as IComparable;
        }

    }
}