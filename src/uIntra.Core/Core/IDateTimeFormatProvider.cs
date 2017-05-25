namespace Compent.uCommunity.Core
{
    public interface IDateTimeFormatProvider
    {
        string TimeFormat { get; set; }
        string DateFormat { get; set; }
        string DateTimeFormat { get; set; }
        string DateTimeValuePickerFormat { get; set; }
        string DatePickerFormat { get; set; }
        string DateTimePickerFormat { get; set; }
    }
}
