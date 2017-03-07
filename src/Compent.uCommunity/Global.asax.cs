using System;
using System.Web.Mvc;
using System.Web.Optimization;
using Compent.uCommunity.App_Start;
using Umbraco.Web;

namespace Compent.uCommunity
{
    public class UcommunityApplication : UmbracoApplication
    {
        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.RegisterMappings();
        }
    }
}
