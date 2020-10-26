namespace Uintra20.Core.Configuration
{
    public interface IConfigurationProvider<out TConfiguration>
    {
        TConfiguration GetSettings();
        void Initialize();
        void Reinitialize();
    }
}