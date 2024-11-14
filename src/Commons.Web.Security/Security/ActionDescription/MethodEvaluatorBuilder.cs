using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Queo.Commons.Web.Security.MethodAuthorize;

namespace Queo.Commons.Web.Security.ActionDescription
{
    internal class MethodEvaluatorBuilder : IEvaluatorBuilder
    {
        private readonly string _actionDescription;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="actionDescription"></param>
        public MethodEvaluatorBuilder(string actionDescription)
        {
            _actionDescription = actionDescription;
        }

        /// <inheritdoc />
        public IActionEvaluator Build(ActionExecutingContext context)
        {
            object methodSecurityRoot = GetAuthorizeService(context);
            MethodInformation methodInformation = new MethodInformation(_actionDescription, context.ActionArguments);
            MethodInfo matchingMethod = FindMatchingMethod(methodInformation, methodSecurityRoot);
            MethodEvaluator methodEvaluator = new MethodEvaluator(matchingMethod, methodInformation, methodSecurityRoot);
            return methodEvaluator;
        }

        private static MethodInfo FindMatchingMethod(MethodInformation methodInformation, object methodSecurityRoot)
        {
            MethodInfo? methodInfo = methodSecurityRoot.GetType()
                .GetMethod(methodInformation.MethodName, methodInformation.MethodParameters.Select(x => x.ParameterType).ToArray());
            if (methodInfo == null)
            {
                throw new MethodAuthorizeException(string.Format("The method {0} could not be found on the object {1}.",
                    methodInformation.MethodName,
                    methodSecurityRoot.GetType().Name));
            }
            return methodInfo;
        }

        private static object GetAuthorizeService(ActionExecutingContext context)
        {
            object? authorizeService = context.HttpContext.RequestServices.GetKeyedService<object>("methodSecurityRoot");
            if (authorizeService == null)
            {
                throw new InvalidOperationException("The service 'methodSecurityRoot' is not registered in the service collection.");
            }
            return authorizeService;
        }
    }
}