namespace uIntra.Core
{
    public static class IntranetConstants
    {
        public static class Common
        {
#warning this two formats related
            public const string DefaultTimeFormat = "HH:mm";
            public const string DefaultDateFormat = "dd.MM.yyyy";
            public const string DefaultDateTimeFormat = "dd.MM.yyyy HH:mm";
            public const string DefaultDateTimeValuePickerFormat = "yyyy-MM-ddTHH:mm";
            public const string DefaultDatePickerFormat = "d.m.Y";
            public const string DefaultDateTimePickerFormat = "d.m.Y H:i";
#warning this two formats related
        }

        public static class Session
        {
            public const string LoggedUserSessionKey = "LoggedUser";
        }
    }
}