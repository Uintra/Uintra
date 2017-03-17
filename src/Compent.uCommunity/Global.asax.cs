using System;
using System.Web.Mvc;
using Compent.uCommunity.App_Start;
using Umbraco.Web;

namespace Compent.uCommunity
{
    public class UcommunityApplication : UmbracoApplication
    {
        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            MapperConfig.RegisterMappings();
        }
    }
}
