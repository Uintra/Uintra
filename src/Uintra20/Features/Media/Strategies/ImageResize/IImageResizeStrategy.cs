namespace Uintra20.Features.Media.Strategies.ImageResize
{
    public interface IImageResizeStrategy
    {
        string Thumbnail { get; }
        string Preview { get; }
        string PreviewTwo { get; }
    }
}