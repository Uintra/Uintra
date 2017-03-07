using uCommunity.Core.App_Plugins.Core.Localization;

namespace Compent.uCommunity.Core
{
    public class LocalizationService : IIntranetLocalizationService
    {
        public string Translate(string key)
        {
            return key + " translation";
        }
    }
}