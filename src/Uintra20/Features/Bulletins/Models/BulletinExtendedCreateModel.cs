using System;
using System.Collections.Generic;

namespace Uintra20.Features.Bulletins.Models
{
    public class BulletinExtendedCreateModel : BulletinCreateModel
    {
        public Guid? GroupId { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}