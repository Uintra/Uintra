namespace uIntra.Core.ApplicationSettings
{
    public interface IApplicationSettings
    {
        string DefaultAvatarPath { get; }

        int PinDaysRangeStart { get; }

        int PinDaysRangeEnd { get; }
    }
}
