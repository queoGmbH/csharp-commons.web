using System;

namespace Commons.Web.Security.MethodAuthorize;

/// <summary>
/// Exception for problems with method authorization.
/// If this exception is thrown, it means that the authorization check is failed.
/// </summary>
public class MethodAuthorizeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodAuthorizeException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public MethodAuthorizeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodAuthorizeException"/> class with the specified error message and inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public MethodAuthorizeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
