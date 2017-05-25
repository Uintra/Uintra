using uCommunity.Core.Localization;

namespace Compent.uIntra.Core
{
    public class LocalizationService : IIntranetLocalizationService
    {
        public string Translate(string key)
        {
            return key + " translation";
        }
    }
}