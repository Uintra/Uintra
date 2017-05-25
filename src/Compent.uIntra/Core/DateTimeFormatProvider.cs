using uIntra.Core;

namespace Compent.uIntra.Core
{
    public class DateTimeFormatProvider : IDateTimeFormatProvider
    {
        public string TimeFormat { get; set; } = "HH:mm";
        public string DateFormat { get; set; } = "dd.MM.yyyy";
        public string DateTimeFormat { get; set; } = "dd.MM.yyyy HH:mm";
        public string DateTimeValuePickerFormat { get; set; } = "yyyy-MM-ddTHH:mm";
        public string DatePickerFormat { get; set; } = "d.m.Y";
        public string DateTimePickerFormat { get; set; } = "d.m.Y H:i";
    }
}