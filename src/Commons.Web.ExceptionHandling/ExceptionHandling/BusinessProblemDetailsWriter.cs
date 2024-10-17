using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Queo.Commons.Web.Exceptions;

/// <summary>
///  Writes the problem details to the response.
/// </summary>
public class BusinessProblemDetailsWriter : IProblemDetailsWriter
{
    private readonly OutputFormatterSelector _formatterSelector;
    private readonly IHttpResponseStreamWriterFactory _writerFactory;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ApiBehaviorOptions _apiBehaviorOptions;

    private static readonly MediaTypeCollection _problemContentTypes = new()
    {
        "application/problem+json",
        "application/problem+xml"
    };

    public BusinessProblemDetailsWriter(OutputFormatterSelector formatterSelector,
        IHttpResponseStreamWriterFactory writerFactory,
        ProblemDetailsFactory problemDetailsFactory,
        IOptions<ApiBehaviorOptions> apiBehaviorOptions)
    {
        this._formatterSelector = formatterSelector;
        this._writerFactory = writerFactory;
        this._problemDetailsFactory = problemDetailsFactory;
        this._apiBehaviorOptions = apiBehaviorOptions.Value;
    }


    public bool CanWrite(ProblemDetailsContext context)
    {
        var controllerAttribute = context.AdditionalMetadata?.GetMetadata<ControllerAttribute>() ??
            context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<ControllerAttribute>();

        return controllerAttribute != null && context.HttpContext.Response.HasStarted == false;
    }

    public ValueTask WriteAsync(ProblemDetailsContext context)
    {
        Exception? exception = context.Exception;
        string errorCode = "broken";
        int statusCode = context.HttpContext.Response.StatusCode;
        string title = "Server error";
        string type = "/problems/server-error";
        Dictionary<string, string> errors = new Dictionary<string, string>();
        if (exception is BusinessException businessException)
        {
            errorCode = businessException.ErrorCode;
            statusCode = businessException.StatusCode;
            title = businessException.Title;
            type = businessException.Type;
            errors = businessException.Errors;
        }


        var apiControllerAttribute = context.AdditionalMetadata?.GetMetadata<IApiBehaviorMetadata>() ??
        context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IApiBehaviorMetadata>();

        if (apiControllerAttribute is null || _apiBehaviorOptions.SuppressMapClientErrors)
        {
            // In this case we don't want to write
            return ValueTask.CompletedTask;
        }

        // Recreating the problem details to get all customizations
        // from the factory
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            statusCode,
            title,
            type,
            context.ProblemDetails.Detail,
            context.ProblemDetails.Instance);

        if (context.ProblemDetails?.Extensions is not null)
        {
            foreach (var extension in context.ProblemDetails.Extensions)
            {
                problemDetails.Extensions[extension.Key] = extension.Value;
            }
        }
        problemDetails.Extensions["errors"] = errors;
        problemDetails.Extensions["errorCode"] = errorCode;

        context.HttpContext.Response.StatusCode = problemDetails.Status ?? statusCode;

        var formatterContext = new OutputFormatterWriteContext(
            context.HttpContext,
            _writerFactory.CreateWriter,
            typeof(ProblemDetails),
            problemDetails);

        var selectedFormatter = _formatterSelector.SelectFormatter(
            formatterContext,
            Array.Empty<IOutputFormatter>(),
            _problemContentTypes);

        if (selectedFormatter == null)
        {
            return ValueTask.CompletedTask;
        }

        return new ValueTask(selectedFormatter.WriteAsync(formatterContext));
    }
}
