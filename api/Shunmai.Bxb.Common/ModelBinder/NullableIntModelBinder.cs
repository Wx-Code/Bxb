using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Shunmai.Bxb.Common.ModelBinder
{
    public class NullableIntModelBinder : IModelBinder
    {
        private readonly ILogger _logger;

        public NullableIntModelBinder(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<NullableIntModelBinder>();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string modelName = bindingContext.ModelName;
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            ModelStateDictionary modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            ModelMetadata metadata = bindingContext.ModelMetadata;
            Type type = metadata.UnderlyingOrModelType;

            try
            {
                string value = valueProviderResult.FirstValue;

                object model;

                if (string.IsNullOrWhiteSpace(value))
                {
                    model = null;
                }
                else if (type == typeof(int))
                {
                    int.TryParse(value, out int result);

                    model = result;
                }
                else
                {
                    throw new NotSupportedException();
                }

                if (model == null && !metadata.IsReferenceOrNullableType)
                {
                    modelState.TryAddModelError(modelName,
                        metadata.ModelBindingMessageProvider
                            .ValueMustBeANumberAccessor(valueProviderResult.ToString()));
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
            catch (Exception exception)
            {
                bool isFormatException = exception is FormatException;

                if (!isFormatException && exception.InnerException != null)
                {
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                }

                modelState.TryAddModelError(modelName, exception, metadata);
            }

            return Task.CompletedTask;
        }
    }
}
