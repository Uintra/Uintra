using Uintra20.Core.Activity;
using Uintra20.Core.Media;

namespace Uintra20.Core.Bulletins
{
    public interface IBulletinsService<TBulletins> : IIntranetActivityService<TBulletins> where TBulletins : BulletinBase
    {
        MediaSettings GetMediaSettings();
    }
}
