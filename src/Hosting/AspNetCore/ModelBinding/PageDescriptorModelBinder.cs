using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Sort;
using Stize.Hosting.AspNetCore.Model;

namespace Stize.Hosting.AspNetCore.ModelBinding
{
    public class PageDescriptorModelBinder : IModelBinder
    {
        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(PageDescriptorModel))
            {
                var model = new PageDescriptorModel();

                bindingContext.Model = model;

                this.BindSkip(bindingContext.ValueProvider, model);
                this.BindTake(bindingContext.ValueProvider, model);
                this.BindEnvelope(bindingContext.ValueProvider, model);
                this.BindFilters(bindingContext.ValueProvider, model);
                this.BindSort(bindingContext.ValueProvider, model);
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            
            return Task.CompletedTask;
        }


        protected void BindSkip(IValueProvider valueProvider, PageDescriptorModel model)
        {
            var offsetResult = valueProvider.GetValue(PageDescriptorModel.SkipKey);
            var value = offsetResult.FirstValue;
            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out var skip))
                {
                    model.Skip = skip;
                }
            }
        }

        protected void BindTake(IValueProvider valueProvider, PageDescriptorModel model)
        {
            var limitResult = valueProvider.GetValue(PageDescriptorModel.TakeKey);
            var value = limitResult.FirstValue;
            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out var take))
                {
                    model.Take = take;
                }
            }
        }

        protected void BindEnvelope(IValueProvider valueProvider, PageDescriptorModel model)
        {
            var envelopeResult = valueProvider.GetValue(PageDescriptorModel.EnvelopeKey);
            var value = envelopeResult.FirstValue;
            if (!string.IsNullOrEmpty(value))
            {
                if (bool.TryParse(value, out var envelope))
                {
                    model.Envelope = envelope;
                }
            }
        }

        protected void BindFilters(IValueProvider valueProvider, PageDescriptorModel model)
        {
            var filterResult = valueProvider.GetValue(PageDescriptorModel.FilterKey);
            var value = filterResult.FirstValue;
            if (!string.IsNullOrEmpty(value))
            {
                var filterValues = value.Split(new[] { ";" }, StringSplitOptions.None);
                foreach (var filterValue in filterValues)
                {
                    var filter = FilterDescriptorFactory.Create(filterValue);
                    if (filter != null)
                    {
                        model.Filters.Add(filter);
                    }
                }
            }
        }

        protected void BindSort(IValueProvider valueProvider, PageDescriptorModel model)
        {
            var sortResult = valueProvider.GetValue(PageDescriptorModel.SortKey);
            var value = sortResult.FirstValue;
            if (!string.IsNullOrEmpty(value))
            {
                var sortValues = value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var sortValue in sortValues)
                {
                    var sort = SortDescriptorFactory.Create(sortValue);
                    if (sort != null)
                    {
                        model.Sorts.Add(sort);
                    }
                }
            }
        }
        
    }
}