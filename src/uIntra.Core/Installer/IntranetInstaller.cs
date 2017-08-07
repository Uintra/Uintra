using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace uIntra.Core.Installer
{
    public class IntranetInstaller
    {
        public void Install(Version installedVersion, Version updatingVersion)
        {
            var installationType = typeof(IIntranetInstallationStep);
            var steps = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(GetTypesSafely)
                .Where(p => installationType.IsAssignableFrom(p) && p.IsClass)
                .Select(st =>
                {
                    var installationStep = Activator.CreateInstance(st) as IIntranetInstallationStep;
                    return new
                    {
                        Step = installationStep,
                        Version = new Version(installationStep?.Version)
                    };
                })
                .Where(el => (el.Version > installedVersion) && (el.Version < updatingVersion))
                .Select(el => el.Step)
                .OrderBy(s => s?.Priority)
                .ToList();

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
