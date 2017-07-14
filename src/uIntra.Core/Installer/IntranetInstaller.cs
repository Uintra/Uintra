using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace uIntra.Core.Installer
{
    public class IntranetInstaller
    {
        public void Install()
        {
            var installationType = typeof(IIntranetInstallationStep);
            var steps = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(GetTypesSafely)
                .Where(p => installationType.IsAssignableFrom(p) && p.IsClass)
                .Select(st => Activator.CreateInstance(st) as IIntranetInstallationStep)
                .OrderBy(s => s?.Priority);

            foreach (var step in steps)
            {
                step.Execute();
            }
        }
        private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }
    }
}
