namespace Uintra.Core.Configuration
{
    public interface IConfigurationProvider<out TConfiguration>
    {
        TConfiguration GetSettings();
        void Initialize();
        void Reinitialize();
    }
}