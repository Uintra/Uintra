using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Users
{
    public class GoogleAuthenticationSettings
    {
        public string ClientId { get; set; }
        public string Domain { get; set; }
        public bool AuthenticationEnabled { get; set; }
    }
}
