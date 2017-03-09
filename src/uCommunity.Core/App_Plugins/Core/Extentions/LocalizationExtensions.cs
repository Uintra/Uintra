namespace uCommunity.Core.App_Plugins.Core.Extentions
{
    public static class LocalizationExtensions
    {
        public static string GetLocalizeKey<T>(this T source)
            where T : struct
        {
            return $"{typeof(T).Name}.{source}";
        }
    }
}