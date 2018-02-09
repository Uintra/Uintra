using System;

namespace Uintra.Core.ModelBinders
{
    public class PropertyBinderAttribute : Attribute
    {
        public Type BinderType { get; private set; }

        public PropertyBinderAttribute(Type binderType)
        {
            BinderType = binderType;
        }
    }
}