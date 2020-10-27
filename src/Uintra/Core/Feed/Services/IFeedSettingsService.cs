using System;
using System.Collections.Generic;
using Uintra.Core.Feed.Settings;

namespace Uintra.Core.Feed.Services
{
    public interface IFeedSettingsService
    {
        FeedSettings GetSettings(Enum type);
        IEnumerable<FeedSettings> GetAllSettings();
    }
}