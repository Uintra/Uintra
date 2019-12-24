using Uintra20.Core.Activity;
using Uintra20.Features.Media;

namespace Uintra20.Features.Bulletins
{
    public interface ISocialsService<TSocials> : IIntranetActivityService<TSocials> where TSocials : SocialBase
    {
        MediaSettings GetMediaSettings();
    }
}
