using System;
using System.Collections.Generic;
using Uintra.Attributes;
using Uintra.Core.Activity.Models;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.Media.Contracts;

namespace Uintra.Features.Social.Models
{
    public class SocialCreateModel :
        IntranetActivityCreateModelBase,
        IContentWithMediaCreateEditModel
    {
        [RequiredVirtual(IsRequired = false)]
        public override string Title { get; set; }
        [RequiredIfEmpty(OtherProperty = nameof(NewMedia)), StringLengthWithoutHtml(2000)]
        public string Description { get; set; }
        public IEnumerable<string> Dates { get; set; }
        [RequiredIfEmpty(OtherProperty = nameof(Description))]
        public string NewMedia { get; set; }
        public int? LinkPreviewId { get; set; }
        public LinkPreviewModel LinkPreview { get; set; }
        public Guid? GroupId { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}