namespace Uintra20.Core
{
    public interface IDateTimeFormatProvider
    {
        string TimeFormat { get; set; }
        string EventDetailsDateFormat { get; set; }
        string EventDetailsTimeFormat { get; set; }
        string EventDetailsTimeWithoutMinutesFormat { get; set; }
        string DateFormat { get; set; }
        string DateTimeFormat { get; set; }
        string DateTimeValuePickerFormat { get; set; }
        string DatePickerFormat { get; set; }
        string DateTimePickerFormat { get; set; }
    }
}
