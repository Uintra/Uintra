using System;
using System.Collections.Generic;

namespace Uintra20.Features.Bulletins.Models
{
    public class BulletinExtendedEditModel : BulletinEditModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}