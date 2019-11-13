namespace Uintra20.Core
{
    public class DateTimeFormatProvider : IDateTimeFormatProvider
    {
        public string TimeFormat { get; set; } = "HH:mm";
        public string EventDetailsDateFormat { get; set; } = "MMM d, yyyy";
        public string EventDetailsTimeFormat { get; set; } = "h.mm tt";
        public string EventDetailsTimeWithoutMinutesFormat { get; set; } = "h tt";
        public string DateFormat { get; set; } = "MMM dd, yyyy";
        public string DateTimeFormat { get; set; } = "MMM dd, yyyy HH:mm";
        public string DateTimeValuePickerFormat { get; set; } = "yyyy-MM-ddTHH:mm";
        public string DatePickerFormat { get; set; } = "d.m.Y";
        public string DateTimePickerFormat { get; set; } = "d.m.Y H:i";
    }
}