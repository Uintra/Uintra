using System.ComponentModel;
using System.Web.Mvc;
using uIntra.Core.Links;

namespace uIntra.Core.ModelBinders
{
    class LinksBinder : ICustomModelBinder
    {
        public const string OverviewFormKey = "links.Overview";
        public const string DetailsNoIdFormKey = "links.DetailsNoId";
        public const string CreatorFormKey = "links.Creator";
        public const string CreateFormKey = "links.Create";


        public object BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            var overviewLink = GetValue(bindingContext.ValueProvider, OverviewFormKey);
            var detailsNoIdLink = GetValue(bindingContext.ValueProvider, DetailsNoIdFormKey);
            var creatorLink = GetValue(bindingContext.ValueProvider, CreatorFormKey);
            var createLink = GetValue(bindingContext.ValueProvider, CreateFormKey);

            var result = new ActivityCreateLinks(
                overview: overviewLink,
                create: createLink,
                creator: creatorLink,
                detailsNoId: detailsNoIdLink
                );

            return result;
        }

        private string GetValue(IValueProvider provider, string key) => provider.GetValue(key)?.AttemptedValue;
    }
}
