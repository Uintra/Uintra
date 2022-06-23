using Microsoft.Extensions.Configuration;
using Ninject;
using Uintra.Search.Configuration;

namespace Uintra.Search.Ioc
{
    public static class SearchRegistration
    {
        public static void RegisterAzureConfiguration(IKernel kernel, string indexPrefix)
        {
            kernel.Bind<IElasticConfigurationSection>().ToMethod(c =>
            {
                var configuration = kernel.Get<IConfiguration>();
                var section = configuration.GetSection(ElasticConfigurationSection.SettingName).Get<ElasticConfigurationSection>();
                section.IndexPrefix = indexPrefix;
                return section;
            }).InSingletonScope();
        }
    }
}
