using uIntra.Core.Activity;

namespace uIntra.Bulletins
{
    public class BulletinCreateFormModel : BulletinCreateModel
    {
        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public string AllowedMediaExtentions { get; set; }
    }
}
