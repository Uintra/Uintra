using Compent.Shared.Extensions;
using System;
using System.Web;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Features.CentralFeed
{
    public class CentralFeedFilterStateService : IFeedFilterStateService<FeedFiltersState>
    {
        private const string CentralFeedFiltersStateCookieName = "centralFeedFiltersState";

        private readonly ICookieProvider _cookieProvider;

        public CentralFeedFilterStateService(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public void SaveFiltersState(FeedFiltersState stateModel)
        {
            var cookie = _cookieProvider.Get(CentralFeedFiltersStateCookieName);
            cookie.Value = stateModel.ToJson();
            _cookieProvider.Save(cookie);
        }

        public FeedFiltersState GetFiltersState()
        {
            var cookie = _cookieProvider.Get(CentralFeedFiltersStateCookieName);
            if (string.IsNullOrEmpty(cookie?.Value))
            {
                cookie = new HttpCookie(CentralFeedFiltersStateCookieName)
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    Value = GetDefaultCentralFeedFiltersState().ToJson()
                };
                _cookieProvider.Save(cookie);
            }
            return cookie.Value.Deserialize<FeedFiltersState>();
        }

        public bool CentralFeedCookieExists()
        {
            return _cookieProvider.Exists(CentralFeedFiltersStateCookieName);
        }

        private FeedFiltersState GetDefaultCentralFeedFiltersState()
        {
            return new FeedFiltersState();
        }        
    }
}