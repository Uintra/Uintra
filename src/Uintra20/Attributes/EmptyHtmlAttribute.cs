using System;
using System.ComponentModel.DataAnnotations;
using Compent.Extensions;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Attributes
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