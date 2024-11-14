using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Queo.Commons.Web.ModelBinding.ExceptionHandling
{
    /// <summary>
    /// Filter to handle ModelBindingException.
    /// </summary>
    public class ModelBindingExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// This method is called when an exception occurs.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public void OnException(ExceptionContext context)
        {
            // Check if the exception is a ModelBindingException
            if (context.Exception is ModelBindingException modelBindingException)
            {
                // Create an anonymous object to hold error information
                var errorInformation = new
                {
                    error = modelBindingException.Message,
                    statusCode = modelBindingException.StatusCode,
                    timestamp = DateTime.UtcNow,
                };

                // Set the result of the context to an ObjectResult with the error information
                context.Result = new ObjectResult(errorInformation)
                {
                    StatusCode = modelBindingException.StatusCode
                };

                // Mark the exception as handled
                context.ExceptionHandled = true;
            }
        }
    }
}
