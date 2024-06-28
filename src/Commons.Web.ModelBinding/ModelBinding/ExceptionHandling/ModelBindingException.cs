using System;

namespace Commons.Web.ModelBinding.ExceptionHandling
{
    /// <summary>
    /// Exception thrown when there is an error during model binding.
    /// </summary>
    public class ModelBindingException : Exception
    {
        /// <summary>
        /// Gets the status code that is associated with the exception.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">The status code that is associated with the exception.</param>
        public ModelBindingException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
