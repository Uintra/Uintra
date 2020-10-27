using System;
using System.ComponentModel.DataAnnotations;
using Compent.Extensions;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmptyHtmlAttribute : ValidationAttribute
    {
        public override bool IsValid(object src)
        {
            return !src.ToString()
                .StripHtml()
                .IsNullOrEmpty();
        }
    }
}