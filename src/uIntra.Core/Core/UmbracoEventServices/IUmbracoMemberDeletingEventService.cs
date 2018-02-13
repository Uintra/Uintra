﻿using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoMemberDeletingEventService
    {
        void ProcessMemberDeleting(IMemberService sender, DeleteEventArgs<IMember> args);
    }
}