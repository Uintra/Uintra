using System.ComponentModel;
using System.Web.Mvc;
using uIntra.Core.Links;

namespace uIntra.Core.ModelBinders
{
    class LinksBinder : ICustomModelBinder
    {
        public const string FeedFormKey = "links.Form";
        public const string OverviewFormKey = "links.Overview";
        public const string DetailsFormKey = "links.Details";
        public const string EditFormKey = "links.Edit";
        public const string DetailsNoIdFormKey = "links.DetailsNoId";
        public const string OwnerFormKey = "links.Owner";
        public const string CreateFormKey = "links.Create"; 
         
        public object BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            var feedLink = GetValue(bindingContext.ValueProvider, FeedFormKey);
            var overviewLink = GetValue(bindingContext.ValueProvider, OverviewFormKey);
            var createLink = GetValue(bindingContext.ValueProvider, CreateFormKey);
            var detailsLink = GetValue(bindingContext.ValueProvider, DetailsFormKey);
            var editLink = GetValue(bindingContext.ValueProvider, EditFormKey);
            var ownerLink = GetValue(bindingContext.ValueProvider, OwnerFormKey);
            var detailsNoIdLink = GetValue(bindingContext.ValueProvider, DetailsNoIdFormKey);

            var result = new ActivityLinks
            {
                Feed = feedLink,
                Overview = overviewLink,
                Create = createLink,
                Details = detailsLink,
                Edit = editLink,
                Owner = ownerLink,
                DetailsNoId = detailsNoIdLink
            };

            return result;
        }

        private string GetValue(IValueProvider provider, string key) => provider.GetValue(key)?.AttemptedValue;

    }
}
