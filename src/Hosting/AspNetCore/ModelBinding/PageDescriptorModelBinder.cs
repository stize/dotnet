using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Model;
using Stize.DotNet.Search.Sort;

namespace Stize.Hosting.AspNetCore.ModelBinding
{
    public class PageDescriptorModelBinder : IModelBinder
    {
        private const string SkipKey = "skip";
        private const string TakeKey = "take";
        private const string EnvelopeKey = "envelope";
        private const string FilterKey = "filters";
        private const string SortKey = "sorts";

        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = new PageDescriptorModel();
            bindingContext.Model = model;
            this.BindSkip(bindingContext.ValueProvider, model);
            this.BindTake(bindingContext.ValueProvider, model);
            this.BindEnvelope(bindingContext.ValueProvider, model);
            this.BindFilters(bindingContext.ValueProvider, model);
            this.BindSort(bindingContext.ValueProvider, model);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }


        protected void BindSkip(IValueProvider valueProvider, PageDescriptorModel model)
        {
            var offsetResult = valueProvider.GetValue(SkipKey);
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
            var limitResult = valueProvider.GetValue(TakeKey);
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
            var envelopeResult = valueProvider.GetValue(EnvelopeKey);
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
            var filterResult = valueProvider.GetValue(FilterKey);
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
            var sortResult = valueProvider.GetValue(SortKey);
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