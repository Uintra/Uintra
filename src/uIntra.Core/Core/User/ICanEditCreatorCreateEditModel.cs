using System;
using System.Collections.Generic;

namespace uIntra.Core.User
{
    public interface ICanEditCreatorCreateEditModel
    {
        Guid CreatorId { get; set; }

        IIntranetUser Creator { get; set; }

        IEnumerable<IIntranetUser> Users { get; set; }

        bool CanEditCreator { get; set; }
    }
}