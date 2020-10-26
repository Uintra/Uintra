using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;
using uSync8.BackOffice;
using uSync8.BackOffice.Configuration;

namespace Uintra.App_Start
{
    //[RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    //[ComposeAfter(typeof(uSync8.Core.uSyncCoreComposer))]
    //[ComposeAfter(typeof(uSync8.ContentEdition.uSyncContentComposer))
    [ComposeAfter(typeof(uSyncBackOfficeComposer))]
    public class UintraComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<UintraApplicationComponent>();
            composition.Components().Append<UintraUmbracoEventComponent>();
        }
    }

    [ComposeAfter(typeof(UBaseline.Core.Composers.ContentAppsComposer))]
    public class UintraContentAppsComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.ContentApps().Remove<UBaseline.Core.ContentApps.ContentPreviewApp>();
        }
    }
}