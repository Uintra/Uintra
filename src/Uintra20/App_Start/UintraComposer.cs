using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Uintra20.App_Start
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class UintraComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            //composition.Register<UmbracoMembersUserManager<UmbracoApplicationMember>>(x =>
            //{
            //    //needs to resolve from Owin
            //    var owinCtx = x.GetInstance<IHttpContextAccessor>().HttpContext.GetOwinContext();
            //    return owinCtx.GetUserManager<UmbracoMembersUserManager<UmbracoApplicationMember>>();
            //}, Lifetime.Request);
            //
            //composition.Register<UmbracoMembersRoleManager<UmbracoApplicationRole>>(x =>
            //{
            //    //needs to resolve from Owin
            //    var owinCtx = x.GetInstance<IHttpContextAccessor>().HttpContext.GetOwinContext();
            //    return owinCtx.GetUserManager<UmbracoMembersRoleManager<UmbracoApplicationRole>>();
            //}, Lifetime.Request);

            composition.Components().Append<UintraApplicationComponent>();
            composition.Components().Append<UintraUmbracoEventComponent>();
        }
    }
}