using Extensions;
using Localization.Core;
using Uintra.Core.Localization;

namespace Compent.Uintra.Core
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
            var translation = _localizationCoreService.Get(key);
            return string.IsNullOrWhiteSpace(translation) ? $"##{key}##" : translation;
        }
    }
}