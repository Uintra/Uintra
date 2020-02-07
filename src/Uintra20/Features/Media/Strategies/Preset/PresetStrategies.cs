namespace Uintra20.Features.Media.Strategies.Preset
{
    public static class PresetStrategies
    {
        public static CentralFeedPresetStrategy ForCentralFeed = new CentralFeedPresetStrategy();
        public static ActivityDetailsPresetStrategy ForActivityDetails = new ActivityDetailsPresetStrategy();
        public static MemberProfilePresetStrategy ForMemberProfile = new MemberProfilePresetStrategy();
    }
}