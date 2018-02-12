namespace Uintra.Core.ApplicationSettings
{
    public interface IApplicationSettings
    {
        string DefaultAvatarPath { get; }
        int MonthlyEmailJobDay { get; }
    }
}
