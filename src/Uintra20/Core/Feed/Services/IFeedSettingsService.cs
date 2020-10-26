using System;
using System.Collections.Generic;
using Uintra20.Core.Feed.Settings;

namespace Uintra20.Core.Feed.Services
{
    public interface IFeedSettingsService
    {
        FeedSettings GetSettings(Enum type);
        IEnumerable<FeedSettings> GetAllSettings();
    }
}