using Microsoft.Extensions.Logging;
using System;

namespace Commons.Web.Security.ActionDescription;

/// <summary>
/// Determines the ActionEvaluatorBuilder that must be used based on the ActionDescription and returns it.
/// </summary>
internal class ActionEvaluatorBuilderFactory
{
    private readonly ILogger<ActionEvaluatorBuilderFactory> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionEvaluatorBuilderFactory"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public ActionEvaluatorBuilderFactory(ILogger<ActionEvaluatorBuilderFactory> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the appropriate evaluator builder based on the action description.
    /// </summary>
    /// <param name="actionDescription">The action description.</param>
    /// <returns>The evaluator builder.</returns>
    public IEvaluatorBuilder GetBuilder(string actionDescription)
    {
        if (IsBusinessIdPermissionExpression(actionDescription))
        {
            return new IdentifierEvaluatorBuilder(actionDescription);
        }
        else if (IsMethodPermissionExpression(actionDescription))
        {
            return new MethodEvaluatorBuilder(actionDescription);
        }
        else if (IsSimplePermissionExpression(actionDescription))
        {
            return new SimpleEvaluatorBuilder(actionDescription);
        }
        string message = string.Format("No suitable builder found for actionDescription: {0}", actionDescription);
        _logger.LogWarning(message);
        throw new InvalidOperationException(message);
    }

    /// <summary>
    /// Checks if the action description represents a simple permission expression.
    /// </summary>
    /// <param name="actionDescription">The action description.</param>
    /// <returns>True if the action description represents a simple permission expression; otherwise, false.</returns>
    private static bool IsSimplePermissionExpression(string actionDescription)
    {
        // We assume that a simple permission contains at least one underscore
        return actionDescription.Contains('_');
    }

    /// <summary>
    /// Checks if the action description represents a method permission expression.
    /// </summary>
    /// <param name="actionDescription">The action description.</param>
    /// <returns>True if the action description represents a method permission expression; otherwise, false.</returns>
    private static bool IsMethodPermissionExpression(string actionDescription)
    {
        // We assume that a method permission contains at least one opening and closing parenthesis
        return actionDescription.Contains('(') && actionDescription.Contains(')') && actionDescription.IndexOf('(') < actionDescription.IndexOf(')');
    }

    /// <summary>
    /// Checks if the action description represents a business id permission expression.
    /// </summary>
    /// <param name="actionDescription">The action description.</param>
    /// <returns>True if the action description represents a business id permission expression; otherwise, false.</returns>
    private static bool IsBusinessIdPermissionExpression(string actionDescription)
    {
        // We assume that a business id permission contains at least one opening and closing curly brace
        return actionDescription.Contains('{') && actionDescription.Contains('}') && actionDescription.IndexOf('{') < actionDescription.IndexOf('}');
    }
}
