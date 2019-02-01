using System;
using Uintra.Users;

namespace Compent.Uintra.Core.Login.Models
{
    public class LoginModel : LoginModelBase
    {
        public GoogleAuthenticationSettings GoogleSettings { get; set; }
        public Version CurrentIntranetVersion { get; set; }

    }
}