using System.ComponentModel.DataAnnotations;
using Uintra20.Core.Extensions;

namespace Uintra20.Attributes
{
    public class StringLengthWithoutHtmlAttribute : ValidationAttribute
    {
        private int MaxLenght { get; }
        public StringLengthWithoutHtmlAttribute(int maxLenght)
        {
            MaxLenght = maxLenght;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var length = value.ToString().Length();

            if (length > MaxLenght)
            {
                var newValueWithoutHtml = value.ToString().StripHtml();

                var newValueLenght = newValueWithoutHtml.Length();
                if (newValueLenght > MaxLenght)
                {
                    return new ValidationResult($"Lenght is more than {MaxLenght}");
                }
            }
            return ValidationResult.Success;

        }
    }
}