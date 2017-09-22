using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace uIntra.Core.ModelBinders
{
    /// <inheritdoc />
    /// <summary>
    /// Allows you to use your custom model binder for specific property through <see cref="T:uIntra.Core.ModelBinders.PropertyBinderAttribute" />
    /// For examples look at <see cref="T:uIntra.Core.ModelBinders.DateTimeBinder" /> usage.
    /// </summary>
    public class CustomModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
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

        /// <summary>
        /// Checks property for <see cref="PropertyBinderAttribute"/>
        /// </summary>
        private static PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes.OfType<PropertyBinderAttribute>().FirstOrDefault();
        }
    }
}