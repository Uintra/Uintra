using System;
using System.Linq;

namespace uIntra.Core.Installer
{
    public class IntranetInstaller
    {
        public void Install()
        {
            var installationType = typeof(IIntranetInstallationStep);
            var steps = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(s => s.GetTypes())
                .Where(p => installationType.IsAssignableFrom(p) && p.IsClass)
                .Select(st => Activator.CreateInstance(st) as IIntranetInstallationStep)
                .OrderBy(s => s?.Priority);

            foreach (var step in steps)
            {
                step.Execute();
            }
        }
    }
}
