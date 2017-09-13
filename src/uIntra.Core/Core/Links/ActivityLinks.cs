using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using uIntra.Core.ModelBinders;

namespace uIntra.Core.Links
{
    public class ActivityLinks : ActivityCreateLinks
    {

        public string Details { get; }
        public string Edit { get; }

        public ActivityLinks(string overview, string create, string details, string edit, string creator, string detailsNoId) : base(overview, create, creator, detailsNoId)
        {
            Details = details;
            Edit = edit;
        }

        public new ActivityLinks WithCreate(string value)
        {
            return new ActivityLinks(Overview, value, Details, Edit, Creator, DetailsNoId);
        }

        public new ActivityLinks WithDetailsNoId(string value)
        {
            return new ActivityLinks(Overview, Create, Details, Edit, Creator, value);
        }

        public ActivityLinks WithEdit(string value)
        {
            return new ActivityLinks(Overview, Edit, Details, value, Creator, DetailsNoId);
        }
    }

    public static class LinksExtensions
    {
        public static MvcHtmlString PassLinks(this HtmlHelper helper, ActivityCreateLinks links)
        {
            string result = String.Empty;

            result += helper.Hidden(LinksBinder.DetailsNoIdFormKey, links.DetailsNoId);
            result += helper.Hidden(LinksBinder.CreateFormKey, links.Create);
            result += helper.Hidden(LinksBinder.CreatorFormKey, links.Creator);
            result += helper.Hidden(LinksBinder.OverviewFormKey, links.Overview);

            return MvcHtmlString.Create(result);
        }
    }
}