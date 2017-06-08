using uIntra.Core.Activity;
using uIntra.Core.Media;

namespace uIntra.Bulletins
{
    public interface IBulletinsService<out TBulletins> : IIntranetActivityService<TBulletins> where TBulletins : BulletinBase
    {
        MediaSettings GetMediaSettings();

        bool CanDelete(IIntranetActivity cached);
    }
}