using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace PitchLogAPI.Attributes
{
    public class MultiFieldModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if(values.Count() == 0)
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var finalValues = new List<string>();

            foreach(string value in values)
            {
                if (value.Contains(","))
                {
                    var splitValues = value.Split(",");
                    finalValues.AddRange(splitValues);
                }
                else
                {
                    finalValues.Add(value);
                }
            }

            bindingContext.Model = finalValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}
