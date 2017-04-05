using System;
using uCommunity.Core.Extentions;

namespace uCommunity.Core
{
    public class AppSettingHelper
    {
        public static T GetAppSetting<T>(string key, bool isRequired = true)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (value.IsNullOrEmpty())
            {
                if (isRequired)
                {
                    throw new ArgumentException($"The key {key} in appSettings is missing!");
                }

                return default(T);
            }

            var result = Convert.ChangeType(value, typeof(T));
            return (T)result;
        }
    }
}
