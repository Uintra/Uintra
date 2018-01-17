using System;
using uIntra.Bulletins;

namespace Compent.uIntra.Core.Bulletins
{
    public class BulletinExtendedCreateModel : BulletinCreateModel
    {
        public Guid? GroupId { get; set; }
        public string TagIdsData { get; set; }
    }
}