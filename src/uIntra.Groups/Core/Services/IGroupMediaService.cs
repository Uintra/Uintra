using System;
using System.Collections.Generic;
using Uintra.Core.Media;
using Umbraco.Core.Models;

namespace Uintra.Groups
{
    public interface IGroupMediaService
    {
        IMedia CreateGroupMedia(string name, byte[] file, Guid groupId);
        IEnumerable<int> CreateGroupMedia(IContentWithMediaCreateEditModel model, Guid groupid, Guid creatorId);
        void GroupTitleChanged(Guid groupId, string newTitle);
    }
}