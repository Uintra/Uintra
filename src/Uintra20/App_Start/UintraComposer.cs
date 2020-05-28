using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Uintra20.App_Start
{
    //[RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    [ComposeAfter(typeof(uSync8.Core.uSyncCoreComposer))]
    //[ComposeBefore(typeof(uSync8.ContentEdition.uSyncContentComposer))]
    public class UintraComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<UintraApplicationComponent>();
            composition.Components().Append<UintraUmbracoEventComponent>();
        }
    }
}