using System;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Navigation.Models
{
    public class TopNavigationItem
    {
        public string Name { get; set; }
        public UintraLinkModel Url { get; set; }
        public Enum Type { get; set; }
    }
}