using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Media;
using Umbraco.Core.Models;

namespace Uintra20.Features.Groups.Services
{
    public interface IGroupMediaService
    {
        IMedia CreateGroupMedia(string name, byte[] file, Guid groupId);
        Task<IMedia> CreateGroupMediaAsync(string name, byte[] file, Guid groupId);
        IEnumerable<int> CreateGroupMedia(IContentWithMediaCreateEditModel model, Guid groupid, Guid creatorId);
        Task<IEnumerable<int>> CreateGroupMediaAsync(IContentWithMediaCreateEditModel model, Guid groupid, Guid creatorId);
        void GroupTitleChanged(Guid groupId, string newTitle);
        Task GroupTitleChangedAsync(Guid groupId, string newTitle);
    }
}