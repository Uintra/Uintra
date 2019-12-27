using Uintra20.Core.Activity;
using Uintra20.Features.Media;

namespace Uintra20.Features.Bulletins
{
    public interface ISocialsService<TBase> : IIntranetActivityService<TBase> where TBase : SocialBase
    {
        MediaSettings GetMediaSettings();
    }
}
