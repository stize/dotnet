using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stize.DotNet.Search.Model;

namespace Stize.Hosting.AspNetCore.ModelBinding
{
    public class PageDescriptorModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(PageDescriptorModel) && context.BindingInfo.BindingSource == BindingSource.Query)
            {
                return new PageDescriptorModelBinder();
            }

            return null;
        }
    }
}