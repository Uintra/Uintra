using Uintra.Core.Activity;
using Uintra.Features.Media;
using Uintra.Features.Media.Models;

namespace Uintra.Features.Social
{
    public interface ISocialService<TSocial> : IIntranetActivityService<TSocial> where TSocial : SocialBase
    {
        MediaSettings GetMediaSettings();
    }
}
