using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stize.Hosting.AspNetCore.Model;

namespace Stize.Hosting.AspNetCore.ModelBinding
{
    public class PageDescriptorModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(PageDescriptorModel))
            {
                return new PageDescriptorModelBinder();
            }

            return null;
        }
    }
}