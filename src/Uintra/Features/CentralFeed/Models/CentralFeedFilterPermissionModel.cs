﻿using System;

namespace Uintra.Features.CentralFeed.Models
{
    public class CentralFeedFilterPermissionModel
    {
        public CentralFeedFilterPermissionModel(
            bool canView,
            Enum permission)
        {
            CanView = canView;
            Permission = permission;
        }

        public bool CanView { get; }
        public Enum Permission { get; }
    }
}