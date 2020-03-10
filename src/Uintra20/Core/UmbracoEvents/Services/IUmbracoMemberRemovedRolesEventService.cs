﻿using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services
{
    public interface IUmbracoMemberRemovedRolesEventService
    {
        void ProcessMemberRemovedRoles(IMemberService sender, RolesEventArgs e);
    }
}