using Ampl.Core;
using System;
using System.Web.Http.ModelBinding;

namespace Ampl.Web.Http
{
    public static class ModelStateExtensions
    {
        public static void ThrowIfNullOrInvalid<T>(this ModelStateDictionary modelState,
                                                   T model,
                                                   string message = null)
            where T : class
        {
            Check.NotNull(modelState, nameof(modelState));

            if (model == null)
            {
                throw new ArgumentException(Messages.ModelIsEmpty);
            }

            if (modelState.IsValid)
            {
                return;
            }

            message = message ?? Messages.ValidationErrorsOccurred;
            throw new ModelStateException(message, modelState);
        }
    }
}
