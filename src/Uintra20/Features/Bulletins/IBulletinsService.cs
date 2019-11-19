using Uintra20.Core.Activity;
using Uintra20.Features.Media;

namespace Uintra20.Features.Bulletins
{
    public interface IBulletinsService<TBulletins> : IIntranetActivityService<TBulletins> where TBulletins : BulletinBase
    {
        MediaSettings GetMediaSettings();
    }
}
