using Umbraco.Core;

namespace uIntra.Bulletins
{
    public class BulletinsSection
    {
        protected const string Alias = "bulletins";
        protected const string Name = "Bulletins";
        protected const string Icon = "icon-notepad _bulletins-section-color";

        public static void AddSectionToAllUsers(ApplicationContext applicationContext)
        {
            var section = applicationContext.Services.SectionService.GetByAlias(Alias);
            if (section != null)
            {
                return;
            }

            applicationContext.Services.SectionService.MakeNew(Name, Alias, Icon, sortOrder: 1);
            applicationContext.Services.UserService.AddSectionToAllUsers(Alias);
        }
    }
}