namespace Uintra20.Features.Media.Strategies.Preset
{
    public class MemberProfilePresetStrategy : IPresetStrategy
    {
        public string ThumbnailPreset { get; } = "preset=profile";
        public string PreviewPreset { get; } = "preset=preview";
        public string PreviewTwoPreset { get; } = "preset=previewTwo";
        public int MediaFilesToDisplay { get; } = int.MaxValue; //Means that all files should render
    }
}