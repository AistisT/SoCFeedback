using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Collections.Generic;

namespace SoCFeedback.Services
{
    public class TrimmingModelBinder : ComplexTypeModelBinder
    {
        public TrimmingModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders) : base(propertyBinders)
        {
        }

        protected override void SetProperty(ModelBindingContext bindingContext, string modelName, ModelMetadata propertyMetadata, ModelBindingResult result)
        {
            var s = result.Model as string;
            if (s != null)
            {
                string resultStr = s.Trim();
                result = ModelBindingResult.Success(resultStr);
            }

            base.SetProperty(bindingContext, modelName, propertyMetadata, result);
        }
    }
}