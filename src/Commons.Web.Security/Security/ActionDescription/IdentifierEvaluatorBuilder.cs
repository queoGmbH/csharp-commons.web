using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Commons.Web.Security.SecurityContextAccessor;

namespace Commons.Web.Security.ActionDescription
{
    internal class IdentifierEvaluatorBuilder : IEvaluatorBuilder
    {
        private readonly string _actionDescription;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="actionDescription"></param>
        public IdentifierEvaluatorBuilder(string actionDescription)
        {
            _actionDescription = actionDescription;
        }

        /// <inheritdoc />
        public IActionEvaluator Build(ActionExecutingContext context)
        {
            ISecurityContextAccessor? securityContextAccessor = context.HttpContext.RequestServices.GetService<ISecurityContextAccessor>();
            if (securityContextAccessor == null)
            {
                throw new InvalidOperationException("SecurityContextAccessor not found in the service collection.");
            }
            IdentifierExpression identifierExpression = new IdentifierExpression(_actionDescription);
            string parameterValue = GetParameterValue(context, identifierExpression);
            SimpleActionEvaluator actionEvaluator = new SimpleActionEvaluator(identifierExpression, parameterValue, securityContextAccessor.GetCurrent());
            return actionEvaluator;
        }

        private static string GetParameterValue(ActionExecutingContext context, IdentifierExpression identifierExpression)
        {
            string parameterName = identifierExpression.ParameterName;
            string? parameterValue = context.RouteData.Values[parameterName]?.ToString();
            if (parameterValue == null)
            {
                string message = string.Format("Parameter {0} not found in the route data.", parameterName);
                throw new InvalidOperationException(message);
            }
            return parameterValue;
        }
    }
}