using System;
using System.Collections.Generic;

namespace Commons.Web.Exceptions;

/// <summary>
///  Exception that is thrown when a business rule is violated.
/// </summary>
/// <remarks>
///  If a specific class is derived, the error parameters should be passed via the constructor.
///  So that you know which values are expected when using it. If necessary, these may be nullable.
/// </remarks>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public BusinessException()
    {
    }

    /// <summary>
    /// The error code is a unique identifier that can be used to retrieve an error text.
    /// </summary>
    public string ErrorCode { get; protected set; } = "0815";

    /// <summary>
    /// The status code of the response.
    /// </summary>
    public int StatusCode { get; protected set; } = 500;

    /// <summary>
    ///  The title of the problem detail response.
    /// </summary>
    public string Title { get; protected set; } = "Business rule validation";

    /// <summary>
    /// The type of the problem detail response.
    /// </summary>
    public string Type { get; protected set; } = "/problems/business-rule-validation";

    /// <summary>
    ///  The dictionary with additional information about the error.
    /// </summary>

    public Dictionary<string, string> Errors { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// Adds an error to the dictionary.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddError(string key, string value)
    {
        Errors.Add(key, value);
    }
}
