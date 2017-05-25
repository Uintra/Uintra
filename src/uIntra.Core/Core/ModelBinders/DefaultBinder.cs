using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace uIntra.Core.ModelBinders
{
    public class CustomModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            // Check if the property has the PropertyBinderAttribute, meaning
            // it's specifying a different binder to use.
            var propertyBinderAttribute = TryFindPropertyBinderAttribute(propertyDescriptor);
            if (propertyBinderAttribute == null)
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
                return;
            }

            var binder = CreateBinder(propertyBinderAttribute);
            var value = binder.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            propertyDescriptor.SetValue(bindingContext.Model, value);
        }

        private static ICustomModelBinder CreateBinder(PropertyBinderAttribute propertyBinderAttribute)
        {
            return (ICustomModelBinder)DependencyResolver.Current.GetService(propertyBinderAttribute.BinderType);
        }

        private static PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes.OfType<PropertyBinderAttribute>().FirstOrDefault();
        }
    }
}