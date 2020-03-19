using Uintra20.Core.Activity;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Models;

namespace Uintra20.Features.Social
{
    public interface ISocialService<TSocial> : IIntranetActivityService<TSocial> where TSocial : SocialBase
    {
        MediaSettings GetMediaSettings();
    }
}
