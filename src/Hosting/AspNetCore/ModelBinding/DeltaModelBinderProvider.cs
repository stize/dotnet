using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stize.DotNet.Delta;

namespace Stize.Hosting.AspNetCore.ModelBinding
{
    public class DeltaModelBinderProvider : IModelBinderProvider
    {

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.IsComplexType && typeof(IDelta).IsAssignableFrom(context.Metadata.ModelType))
            {
                return new DeltaModelBinder();
            }

            return null;
           
        }
    }
}