using System.Collections.Generic;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;

namespace Uintra20.Core.Activity.Models
{
    public class IntranetActivityDetailsViewModel : IntranetActivityPreviewModelBase, ICommentable
    {
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}