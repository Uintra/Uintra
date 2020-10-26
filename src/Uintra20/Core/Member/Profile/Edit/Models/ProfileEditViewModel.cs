using System;
using System.Collections.Generic;

namespace Uintra20.Core.Member.Profile.Edit.Models
{
    public class ProfileEditViewModel : ProfileModel
    {
        public IDictionary<Enum, bool> MemberNotifierSettings { get; set; }
    }
}