using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Compent.uCommunity.Core
{
    public class DateTimeFormatProvider : IDateTimeFormatProvider
    {
        public string DefaultTimeFormat { get; set; } = "HH:mm";
        public string DefaultDateFormat { get; set; } = "dd.MM.yyyy";
        public string DefaultDateTimeFormat { get; set; } = "dd.MM.yyyy HH:mm";
        public string DefaultDateTimeValuePickerFormat { get; set; } = "yyyy-MM-ddTHH:mm";
        public string DefaultDatePickerFormat { get; set; } = "d.m.Y";
        public string DefaultDateTimePickerFormat { get; set; } = "d.m.Y H:i";
    }
}