using System;
using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupMediaService
    {
        IMedia CreateGroupMedia(string name, byte[] file, Guid groupId);
        void GroupTitleChanged(Guid groupId, string newTitle);
    }
}