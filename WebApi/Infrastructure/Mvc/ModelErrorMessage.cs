using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;


namespace WebApi.Infrastructure.Mvc
{
    public class ModelErrorMessage
    {
        private readonly ModelError _modelError;

        public ModelErrorMessage(ModelError modelError)
        {
            _modelError = modelError;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_modelError.ErrorMessage))
            {
                return _modelError.ErrorMessage;
            }

            if (_modelError.Exception is JsonReaderException readerException)
            {
                return readerException.Message;
            }

            return "Could not to read the request body.";
        }
    }
}