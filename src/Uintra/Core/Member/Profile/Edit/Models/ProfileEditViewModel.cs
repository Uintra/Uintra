using System;
using System.Collections.Generic;

namespace Uintra.Core.Member.Profile.Edit.Models
{
    public class ProfileEditViewModel : ProfileModel
    {
        public IDictionary<Enum, bool> MemberNotifierSettings { get; set; }
    }
}