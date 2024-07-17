using Microsoft.Extensions.DependencyInjection;
using System;

namespace Commons.Web.Security.MethodAuthorize;

/// <summary>
/// Represents a configuration class for method authorization.
/// </summary>
public static class MethodAuthorizeConfiguration
{
    /// <summary>
    /// Adds the method authorization configuration to the service collection.
    /// </summary>
    /// <typeparam name="T">The type of the authorization root.</typeparam>
    /// <param name="services">The service collection.</param>
    public static void AddMethodAuthorize<T>(this IServiceCollection services) where T : class
    {
        services.AddScoped<T>();
        // delegate to retrieve the authorization root registered before
        Func<IServiceProvider, object, T> func = (sp, root) =>
        {
            var methodSecurityRoot = sp.GetRequiredService<T>();
            return methodSecurityRoot;
        };
        services.AddKeyedScoped<object>("methodSecurityRoot", func);
    }
}
