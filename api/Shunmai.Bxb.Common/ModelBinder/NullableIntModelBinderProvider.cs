
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shunmai.Bxb.Common.ModelBinder
{
    public class NullableIntModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Type modelType = context.Metadata.UnderlyingOrModelType;

            ILoggerFactory loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();

            if (modelType == typeof(int))
            {
                return new NullableIntModelBinder(loggerFactory);
            }

            return null;
        }
    }
}
