using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// Extends the original response with additional information.
    /// The additional information is determined by the action description attributes and describe the permissions needed to execute the different actions.
    /// </summary>
    public class ActionDecoratorFilter : IActionFilter
    {
        private const string QUERY_PARAMETER_NAME = "additionalInformation";

        private List<EvaluationResult> _evaluationResults = [];
        private bool _shouldBeDecorated;

        /// <summary>
        /// Executes after the action method is executed.
        /// </summary>
        /// <param name="context">The context of the executed action.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Should the result decorated?
            if (_shouldBeDecorated)
            {
                ObjectResult? objectResult = context.Result as ObjectResult;
                if (objectResult == null)
                {
                    throw new InvalidOperationException("Result is not an ObjectResult.");
                }
                var originalResult = objectResult.Value;
                ActionDecoratorResult actionDecoratorResult;
                if (IsDisabledByQueryParam(context))
                {
                    actionDecoratorResult = new ActionDecoratorResult(originalResult, []);
                }
                else
                {
                    actionDecoratorResult = new ActionDecoratorResult(originalResult, _evaluationResults.ToDictionary(x => x.Name, v => v.CanExecute));
                }
                objectResult.Value = actionDecoratorResult;
            }
        }

        /// <summary>
        /// Executes before the action method is executed.
        /// </summary>
        /// <param name="context">The context of the executing action.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _shouldBeDecorated = ShouldResultBeDecorated(context);
            if (_shouldBeDecorated)
            {
                List<string> actionDescriptions = GetActionDescriptions(context);
                ActionEvaluatorBuilderFactory? factory = GetEvaluatorBuilderFactory(context) ?? throw new InvalidOperationException("ActionEvaluatorBuilderFactory not found in the service collection.");
                List<IEvaluatorBuilder> builders = GetBuilders(actionDescriptions, factory);
                _evaluationResults = GetEvaluationResults(context, builders);
            }
        }

        private static List<EvaluationResult> GetEvaluationResults(ActionExecutingContext context, List<IEvaluatorBuilder> builders)
        {
            List<EvaluationResult> evaluationResults = new List<EvaluationResult>();
            foreach (var builder in builders)
            {
                EvaluationResult evaluationResult = builder.Build(context).Evaluate();
                evaluationResults.Add(evaluationResult);
            }
            return evaluationResults;
        }

        private static List<IEvaluatorBuilder> GetBuilders(List<string> actionDescriptions, ActionEvaluatorBuilderFactory factory)
        {
            List<IEvaluatorBuilder> builders = new List<IEvaluatorBuilder>();
            foreach (var actionDescription in actionDescriptions)
            {
                IEvaluatorBuilder builder = factory.GetBuilder(actionDescription);
                builders.Add(builder);
            }
            return builders;
        }

        private static List<string> GetActionDescriptions(ActionExecutingContext context)
        {
            IEnumerable<ActionDescriptionAttribute> actionDescriptions = GetActionDescriptionAttributes(context);
            List<string> descriptions = new List<string>();
            foreach (var actionDescription in actionDescriptions)
            {
                descriptions.AddRange(actionDescription.Permissions);
            }
            // remove possible duplicates
            descriptions = descriptions.Distinct().ToList();

            return descriptions;
        }

        private static ActionEvaluatorBuilderFactory? GetEvaluatorBuilderFactory(ActionExecutingContext context)
        {
            ActionEvaluatorBuilderFactory? actionEvaluatorBuilderFactory = context.HttpContext.RequestServices.GetService<ActionEvaluatorBuilderFactory>();
            return actionEvaluatorBuilderFactory;
        }

        private static bool ShouldResultBeDecorated(ActionExecutingContext context)
        {
            bool shouldResultDecorated = context.HttpContext.Response.HasStarted == false &&
                context.HttpContext.Response.StatusCode >= 200 &&
                HasActionDescriptionAttribute(context);

            return shouldResultDecorated;
        }

        private static bool IsDisabledByQueryParam(ActionExecutedContext context)
        {
            // it is disabled if the query parameter is present and set to false
            return context.HttpContext.Request.Query.ContainsKey(QUERY_PARAMETER_NAME) &&
                context.HttpContext.Request.Query[QUERY_PARAMETER_NAME] == "false";
        }

        private static bool HasActionDescriptionAttribute(ActionExecutingContext context)
        {
            return GetActionDescriptionAttributes(context).Any();
        }

        private static IEnumerable<ActionDescriptionAttribute> GetActionDescriptionAttributes(ActionExecutingContext context)
        {
            var actionDescriptionAttributes = context.ActionDescriptor.EndpointMetadata
                .Where(a => a.GetType() == typeof(ActionDescriptionAttribute))
                .Cast<ActionDescriptionAttribute>();
            return actionDescriptionAttributes;
        }
    }
}