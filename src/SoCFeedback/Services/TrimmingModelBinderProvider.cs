using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SoCFeedback.Services
{
    public class TrimmingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                var propertyBinders = context.Metadata.Properties.ToDictionary(property => property, context.CreateBinder);

                return new TrimmingModelBinder(propertyBinders);
            }

            return null;
        }
    }
}
