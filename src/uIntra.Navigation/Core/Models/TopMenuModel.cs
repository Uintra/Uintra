﻿using Uintra.Core.User;

namespace Uintra.Navigation
{
    public class TopNavigationModel
    {
        public IIntranetUser CurrentUser { get; set; }
        public string CentralUserListUrl { get; set; }
    }
}
