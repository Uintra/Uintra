﻿using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Features.Navigation.Models
{
    public class UserListLinkModel
    {
        public IPublishedContent ContentPage { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}