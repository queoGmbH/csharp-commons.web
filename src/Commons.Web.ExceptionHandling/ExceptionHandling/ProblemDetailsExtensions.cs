using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.Web.Exceptions;
public static class ProblemDetailsExtensions
{
    /// <summary>
    /// Adds conponents for extended problem details, so that they formatted like rfc 7808.
    /// </summary>
    /// <remarks>
    /// You have to add an exception handling middleware to use this feature. Especially in production environments.
    /// There you can use <code>app.UseExceptionHandler();</code>.
    /// </remarks>
    /// <param name="services"></param>
    public static void UseExtendedProblemDetails(this IServiceCollection services)
    {
        services.AddSingleton<IProblemDetailsWriter, BusinessProblemDetailsWriter>();
        services.AddTransient<ProblemDetailsFactory, BusinessProblemDetailsFactory>();
        services.AddProblemDetails();
    }
}
