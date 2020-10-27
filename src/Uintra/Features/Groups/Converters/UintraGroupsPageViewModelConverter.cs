﻿using System;
using UBaseline.Core.Node;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Models;

namespace Uintra.Features.Groups.Converters
{
    public class UintraGroupsPageViewModelConverter : INodeViewModelConverter<UintraGroupsPageModel, UintraGroupsPageViewModel>
    {
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsPageViewModelConverter(IGroupHelper groupHelper)
        {
            _groupHelper = groupHelper;
        }

        public void Map(UintraGroupsPageModel node, UintraGroupsPageViewModel viewModel)
        {
            viewModel.Navigation = _groupHelper.GroupNavigation();
        }
    }
}