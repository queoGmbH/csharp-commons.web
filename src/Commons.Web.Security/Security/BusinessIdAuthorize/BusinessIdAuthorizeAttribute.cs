using Microsoft.AspNetCore.Mvc.Filters;

using Commons.Web.Security.SecurityContextAccessor;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.Web.Security.BusinessIdAuthorize;

/// <summary>
/// The attribute is used to perform authorization based on the identifier in the route.
/// It only works for precomputed permissions. The permission must be in the form of "can_read_entity_{businessId}".
/// </summary>
/// <example>
/// <code>
/// [Route("documents/{businessId}")]
/// [BusinessIdAuthorize("can_read_entity_{businessId}")]
/// public IActionResult Update(Document document) ... 
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method)]
public class BusinessIdAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _authorizationExpression;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessIdAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="authorizationExpression">The authorization expression.</param>
    public BusinessIdAuthorizeAttribute(string authorizationExpression)
    {
        _authorizationExpression = authorizationExpression;
    }

    /// <summary>
    /// Called when the authorization is being performed.
    /// </summary>
    /// <param name="context">The authorization filter context.</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ISecurityContext? securityContext = context.HttpContext.RequestServices.GetService<ISecurityContextAccessor>()?.GetCurrent();
        if (securityContext == null)
        {
            throw new InvalidOperationException("SecurityContext not found");
        }
        // get the parameter name from the expression
        string businessIdParameterName = _authorizationExpression.Substring(_authorizationExpression.IndexOf('{') + 1,
            _authorizationExpression.IndexOf('}') - _authorizationExpression.IndexOf('{') - 1);
        // get the value of the parameter from the route data
        string? businessId = context.RouteData.Values[businessIdParameterName]?.ToString();
        if (string.IsNullOrWhiteSpace(businessId))
        {
            throw new InvalidOperationException($"Parameter {businessIdParameterName} not found in RouteData");
        }
        // construct the needed permission 
        string neededPermission = _authorizationExpression.Replace($"{{{businessIdParameterName}}}", businessId);
        if (!securityContext.HasPermission(neededPermission))
        {
            throw new UnauthorizedAccessException($"User does not have permission {neededPermission}");
        }
    }
}
