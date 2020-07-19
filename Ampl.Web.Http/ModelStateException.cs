using System;
using System.Web.Http.ModelBinding;

namespace Ampl.Web.Http
{
    public class ModelStateException : InvalidOperationException
    {
        public ModelStateDictionary ModelState { get; private set; }

        public ModelStateException() : base()
        { }

        public ModelStateException(string message) : base(message)
        { }

        public ModelStateException(string message, ModelStateDictionary modelState) : base(message)
        {
            ModelState = modelState;
        }
    }
}
