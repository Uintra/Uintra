using System.Collections.Generic;
using Uintra20.Features.CentralFeed.Models;

namespace Uintra20.Features.CentralFeed.Commands
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