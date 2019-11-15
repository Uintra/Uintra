using LightInject;
using Umbraco.Core.Composing.LightInject;

namespace Uintra20.Infrastructure.Ioc
{
    public class LightInjectContainerProvider : LightInjectContainer
    {
        public IServiceContainer ServiceContainer { get; }
        public LightInjectContainerProvider(ServiceContainer container) : base(container)
        {
            ServiceContainer = container;
        }
        
    }
}