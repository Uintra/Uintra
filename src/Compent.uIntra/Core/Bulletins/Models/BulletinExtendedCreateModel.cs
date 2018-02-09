using System;
using Uintra.Bulletins;

namespace Compent.Uintra.Core.Bulletins
{
    public class BulletinExtendedCreateModel : BulletinCreateModel
    {
        public Guid? GroupId { get; set; }
        public string TagIdsData { get; set; }
    }
}