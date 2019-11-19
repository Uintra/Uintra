namespace Uintra20.Features.Comments.Models
{
    public class CommentPreviewModel
    {
        public int Count { get; set; }

        public string Link { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsExistsUserComment { get; set; }
    }
}