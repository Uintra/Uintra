using Localization.Core;
using uIntra.Core.Localization;

namespace Compent.uIntra.Core
{
    public class LocalizationService : IIntranetLocalizationService
    {
        private readonly ILocalizationCoreService _localizationCoreService;

        public LocalizationService(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public string Translate(string key)
        {
            return _localizationCoreService.Get(key);
        }
    }
}