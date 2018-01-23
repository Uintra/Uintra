
namespace Compent.uIntra.Core.Updater
{
    public interface IMigrationStepsResolver
    {
        T Resolve<T>() where T: class;
    }
}