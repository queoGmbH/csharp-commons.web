using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Commons.Web.Security.SecurityContextAccessor;

namespace Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// Simple evaluator builder.
    /// </summary>
    internal class SimpleEvaluatorBuilder : IEvaluatorBuilder
    {
        private readonly string _actionDescription;

        /// <summary>
        /// ctor.
        /// </summary>
        public SimpleEvaluatorBuilder(string actionDescription)
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

            return new SimpleActionEvaluator(_actionDescription, securityContextAccessor.GetCurrent());
        }
    }
}