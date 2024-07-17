using Microsoft.AspNetCore.Mvc.Filters;

namespace Commons.Web.Security.ActionDescription;

/// <summary>
/// Interface for types that can build an ActionEvaluator.
/// </summary>
internal interface IEvaluatorBuilder
{
    /// <summary>
    /// Builds an ActionEvaluator that can be used to evaluate the action.
    /// </summary>
    /// <param name="context"></param>
    /// <returns>The ActionEvaluator</returns>
    IActionEvaluator Build(ActionExecutingContext context);
}
