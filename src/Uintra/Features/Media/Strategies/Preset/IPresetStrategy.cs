namespace Uintra.Features.Media.Strategies.Preset
{
    public interface IPresetStrategy
    {
        string ThumbnailPreset { get; }
        string PreviewPreset { get; }
        string PreviewTwoPreset { get; }
        int MediaFilesToDisplay { get; }
    }
}