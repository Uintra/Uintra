using System;

namespace Uintra20.Core.Bulletins
{
    public class BulletinExtendedCreateModel : BulletinCreateModel
    {
        public Guid? GroupId { get; set; }
        public string TagIdsData { get; set; }
    }
}