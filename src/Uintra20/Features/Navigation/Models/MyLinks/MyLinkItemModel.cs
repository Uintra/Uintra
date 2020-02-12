using System;

namespace Uintra20.Features.Navigation.Models.MyLinks
{
    public class MyLinkItemModel
    {
        public Guid Id { get; set; }
        public int ContentId { get; set; }
        public Guid? ActivityId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}