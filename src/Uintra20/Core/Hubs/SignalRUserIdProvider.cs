using System.Security.Claims;
using Microsoft.AspNet.SignalR;

namespace Uintra20.Core.Hubs
{
    public class SignalRUserIdProvider: IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return ((ClaimsIdentity) request.User.Identity).FindFirst("UserId").Value;
        }
    }
}