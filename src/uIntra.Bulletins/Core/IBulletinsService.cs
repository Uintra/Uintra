using Uintra.Core.Activity;
using Uintra.Core.Media;

namespace Uintra.Bulletins
{
    public interface IBulletinsService<out TBulletins> : IIntranetActivityService<TBulletins> where TBulletins : BulletinBase
    {
        MediaSettings GetMediaSettings();
    }
}