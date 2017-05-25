using System.ComponentModel;
using System.Web.Mvc;

namespace uIntra.Core.ModelBinders
{
    public interface ICustomModelBinder
    {
        object BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor);
    }
}