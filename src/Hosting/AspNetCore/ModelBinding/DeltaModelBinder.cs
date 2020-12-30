using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Delta;
using Stize.DotNet.Json;

namespace Stize.Hosting.AspNetCore.ModelBinding
{
    public class DeltaModelBinder : IModelBinder
    {

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            
            if (typeof(IDelta).IsAssignableFrom(bindingContext.ModelType)
                && bindingContext.ModelType.GetGenericTypeDefinition() == typeof(Delta<>))
            {
                var serializer = bindingContext.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
                bindingContext.HttpContext.Request.EnableBuffering();
                var delta = (IDelta)await serializer.DeserializeAsync(bindingContext.HttpContext.Request.Body, bindingContext.ModelType, default);
                
                var validator = bindingContext.HttpContext.RequestServices?.GetService<IObjectModelValidator>();
                if (validator != null)
                {
                    var deltaOfT = (dynamic)delta;
                    var instanceOfT = deltaOfT.GetInstance();
                    deltaOfT.Patch(instanceOfT);
                    validator.Validate(bindingContext.ActionContext, null, string.Empty, instanceOfT);
                    foreach (var key in bindingContext.ActionContext.ModelState.Keys)
                    {
                        if (!delta.GetChangedPropertyNames().Contains(key))
                        {
                            bindingContext.ActionContext.ModelState.Remove(key);
                        }
                    }
                }


                bindingContext.Model = delta;
                bindingContext.Result = ModelBindingResult.Success(delta);
            }

        }
    }
}