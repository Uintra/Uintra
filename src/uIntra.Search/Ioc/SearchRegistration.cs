using Compent.Shared.Configuration.Builders.Azure.Helpers;
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
                var section = AzureConfigSectionHelper.GetConfigSection<ElasticConfigurationSection>();
                section.IndexPrefix = indexPrefix;
                return section;
            }).InSingletonScope();
        }
    }
}
