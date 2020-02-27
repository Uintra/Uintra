using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using UBaseline.Core.Extensions;
using Umbraco.Core.Logging;

namespace Uintra20.Core.Hubs
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        private readonly ILogger _logger;

        public SignalRUserIdProvider(ILogger logger)
        {
            _logger = logger;
        }
        public string GetUserId(IRequest request)
        {
            if (request.User?.Identity == null)
            {
                _logger.Error(typeof(SignalRUserIdProvider), $"Empty user identity - {request.ToJson()}");
                return string.Empty;
            }

            if (request.User.Identity is ClaimsIdentity identity)
            {
                var claim = identity.FindFirst("UserId");
                if (claim != null)
                {
                    return claim.Value;
                }
            }
            _logger.Error(typeof(SignalRUserIdProvider), $"SignalRUserIdProvider. Not claims identity or empty UserId claim - {request.User.Identity.ToJson()}");
            return string.Empty;
        }
    }
}