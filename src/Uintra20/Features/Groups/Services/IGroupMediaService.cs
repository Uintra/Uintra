using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UBaseline.Shared.Media;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Services
{
    public interface IGroupMediaService
    {
        IMediaModel CreateGroupMedia(string name, byte[] file, Guid groupId);
        Task<IMediaModel> CreateGroupMediaAsync(string name, byte[] file, Guid groupId);
        IEnumerable<int> CreateGroupMedia(IContentWithMediaCreateEditModel model, Guid groupId, Guid creatorId);
        Task<IEnumerable<int>> CreateGroupMediaAsync(IContentWithMediaCreateEditModel model, Guid groupId, Guid creatorId);
        void GroupTitleChanged(Guid groupId, string newTitle);
        Task GroupTitleChangedAsync(Guid groupId, string newTitle);
    }
}