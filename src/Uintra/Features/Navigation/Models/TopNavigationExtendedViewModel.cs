using System;

namespace Uintra.Features.Navigation.Models
{
    public class TopNavigationExtendedViewModel : TopNavigationViewModel
    {
        public Uri UintraDocumentationLink { get; set; }
        public Version UintraDocumentationVersion { get; set; }
    }
}