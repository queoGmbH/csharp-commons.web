using Microsoft.Extensions.DependencyInjection;

using Commons.Web.Security.SecurityContextAccessor;

namespace Commons.Web.Security
{
    /// <summary>
    /// Provides methods to configure the security context services.
    /// </summary>
    public static class SecurityContextConfiguration
    {
        /// <summary>
        /// Adds the security context services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSecurityContext">The type of the security context.</typeparam>
        /// <typeparam name="TSecurityContextFactory">The type of the security context factory.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        public static void AddSecurityContextServices<TSecurityContext, TSecurityContextFactory>(this IServiceCollection services) where TSecurityContext : class, ISecurityContext where TSecurityContextFactory : class, ISecurityContextFactory
        {
            // We need to register the same instance of SecurityContextHolder for the ISecurityContextHolder and ISecurityContextInvalidator
            services.AddSingleton<SecurityContextHolder>();
            services.AddSingleton<ISecurityContextHolder, SecurityContextHolder>(sp => sp.GetRequiredService<SecurityContextHolder>());
            services.AddSingleton<ISecurityContextInvalidator, SecurityContextHolder>(sp => sp.GetRequiredService<SecurityContextHolder>());
            services.AddScoped<SecurityContextCreator>();
            services.AddScoped<ISecurityContext, TSecurityContext>();
            services.AddScoped<ISecurityContextAccessor<ISecurityContext>, StaticWebSecurityContextAccessor>();
            services.AddScoped<ISecurityContextAccessor, StaticWebSecurityContextAccessor>(sp => (StaticWebSecurityContextAccessor)sp.GetRequiredService<ISecurityContextAccessor<ISecurityContext>>());
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<ISecurityContextFactory, TSecurityContextFactory>();
        }
    }
}
