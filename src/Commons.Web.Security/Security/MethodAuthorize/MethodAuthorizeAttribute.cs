using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.Web.Security.MethodAuthorize
{
    /// <summary>
    /// This attribute is used to perform authorization via a method call.
    /// Don't use it for sides that are cached, because this attribute is an action filter
    /// which is called after the cache filter.
    /// </summary>
    /// <remarks>
    /// You can specify the method to call for authorization by providing an expression.
    /// Parameters must have a trailing # to indicate that they are parameters. They also
    /// have to be in the same order as the method parameters.
    /// If you want to use a fixed string, put it in single quotation marks.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAuthorizeAttribute : Attribute, IActionFilter
    {
        private readonly string _methodExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="methodExpression">The method expression.</param>
        public MethodAuthorizeAttribute(string methodExpression)
        {
            _methodExpression = methodExpression;
        }

        /// <summary>
        /// Called after the action method is executed.
        /// </summary>
        /// <param name="context">The context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// Called before the action method is executed.
        /// </summary>
        /// <param name="context">The context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // get the object with name "methodSecurityRoot" from the service collection
            object methodSecurityRoot = GetAuthorizeService(context);
            bool isAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                throw new MethodAuthorizeException("The user is not authenticated.");
            }
            bool isAuthorized = false;

            MethodInformation methodInformation = new MethodInformation(_methodExpression, context.ActionArguments);
            MethodInfo matchingMethod = FindMatchingMethod(methodInformation, methodSecurityRoot);

            try
            {
                isAuthorized = (bool)matchingMethod.Invoke(methodSecurityRoot, methodInformation.MethodParameters.Select(x => x.ParameterValue).ToArray());
            }
            catch (Exception ex)
            {
                throw new MethodAuthorizeException("The method for authorizing the request could not be called successfully.", ex);
            }

            if (!isAuthorized)
            {
                string message = string.Format("The user is not authorized to execute the method {0}.", context.ActionDescriptor.DisplayName);
                throw new MethodAuthorizeException(message);
            }
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