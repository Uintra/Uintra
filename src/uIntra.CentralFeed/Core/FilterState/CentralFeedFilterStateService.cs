using System;
using System.Web;
using Uintra.Core;
using Uintra.Core.Extensions;

namespace Uintra.CentralFeed
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
                    Expires = DateTime.Now.AddDays(7),
                    Value = GetDefaultCentralFeedFiltersState().ToJson(),
                    HttpOnly = true,
                    Secure = true
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
            return new FeedFiltersState
            {
                BulletinFilterSelected = true
            };
        }        
    }
}