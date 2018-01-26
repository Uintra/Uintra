using System.Web.Mvc;

namespace Compent.uIntra.Core.Updater
{
    public class MigrationStepsResolver : IMigrationStepsResolver
    {
        public T Resolve<T>() where T : class
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}