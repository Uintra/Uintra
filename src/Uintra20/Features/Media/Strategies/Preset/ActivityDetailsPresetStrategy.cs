namespace Uintra20.Features.Media.Strategies.Preset
{
    public class ActivityDetailsPresetStrategy : IPresetStrategy
    {
        public string ThumbnailPreset { get; } = "preset=thumbnail";
        public string PreviewPreset { get; } = "preset=preview";
        public string PreviewTwoPreset { get; } = "preset=previewTwo";
        public int MediaFilesToDisplay { get; } = int.MaxValue; //Means that all files should render
    }
}