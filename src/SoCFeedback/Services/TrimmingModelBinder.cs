using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace SoCFeedback.Services
{
    public class TrimmingModelBinder : ComplexTypeModelBinder
    {
        public TrimmingModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders) : base(propertyBinders)
        {
        }

        protected override void SetProperty(ModelBindingContext bindingContext, string modelName,
            ModelMetadata propertyMetadata, ModelBindingResult result)
        {
            var s = result.Model as string;
            if (s != null)
            {
                var resultStr = s.Trim();
                result = ModelBindingResult.Success(resultStr);
            }

            base.SetProperty(bindingContext, modelName, propertyMetadata, result);
        }
    }
}