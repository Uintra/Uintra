using System.Collections.Generic;
using Uintra.Features.CentralFeed.Models;

namespace Uintra.Features.CentralFeed.Commands
{
    public class CentralFeedFilterCommand
    {
        public CentralFeedFilterCommand(IReadOnlyCollection<CentralFeedFilterPermissionModel> list)
        {
            CentralFeedPermissions = list;
        }

        public IReadOnlyCollection<CentralFeedFilterPermissionModel> CentralFeedPermissions { get; }
    }
}