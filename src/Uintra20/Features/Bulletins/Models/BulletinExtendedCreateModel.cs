using System;

namespace Uintra20.Features.Bulletins.Models
{
    public class BulletinExtendedCreateModel : BulletinCreateModel
    {
        public Guid? GroupId { get; set; }
        public string TagIdsData { get; set; }
    }
}