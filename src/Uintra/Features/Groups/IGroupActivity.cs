﻿using System;

namespace Uintra.Features.Groups
{
    public interface IGroupActivity
    {
        Guid Id { get; set; }
        Guid? GroupId { get; set; }
        Guid CreatorId { get; set; }
    }
}
