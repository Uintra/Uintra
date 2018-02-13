using System;
using Umbraco.Core.Models;

namespace Uintra.Groups
{
    public interface IGroupMediaService
    {
        IMedia CreateGroupMedia(string name, byte[] file, Guid groupId);
        void GroupTitleChanged(Guid groupId, string newTitle);
    }
}