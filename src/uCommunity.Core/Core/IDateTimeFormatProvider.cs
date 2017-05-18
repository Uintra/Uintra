namespace Compent.uCommunity.Core
{
    public interface IDateTimeFormatProvider
    {
        string DefaultTimeFormat { get; set; }
        string DefaultDateFormat { get; set; }
        string DefaultDateTimeFormat { get; set; }
        string DefaultDateTimeValuePickerFormat { get; set; }
        string DefaultDatePickerFormat { get; set; }
        string DefaultDateTimePickerFormat { get; set; }
    }
}
