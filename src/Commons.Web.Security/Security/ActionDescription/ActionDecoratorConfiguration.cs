using Microsoft.Extensions.DependencyInjection;

namespace Commons.Web.Security.ActionDescription;

/// <summary>
/// Provides configuration for the action decorator.
/// </summary>
public static class ActionDecoratorConfiguration
{
    /// <summary>
    /// Adds the required services for the action decorator.
    /// The filter must be added when configuring the controller for it to work.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddActionDecorator(this IServiceCollection services)
    {
        services.AddScoped<ActionEvaluatorBuilderFactory>();
    }
}
