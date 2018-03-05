namespace Uintra.Comments
{
    public class CommentPreviewModel
    {
        public int Count { get; set; }

        public string Link { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsExistsUserComment { get; set; }
    }
}