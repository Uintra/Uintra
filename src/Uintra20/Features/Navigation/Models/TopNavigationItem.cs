using System;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Navigation.Models
{
    public class TopNavigationItem
    {
        public string Name { get; set; }
        public UintraLinkModel Url { get; set; }
        public Enum Type { get; set; }
    }
}