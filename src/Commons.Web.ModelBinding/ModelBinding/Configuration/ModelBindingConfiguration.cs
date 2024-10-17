using Microsoft.Extensions.DependencyInjection;

namespace Queo.Commons.Web.ModelBinding.Configuration
{
    /// <summary>
    /// Provides a static class for configuring model binding.
    /// </summary>
    public static class ModelBindingConfiguration
    {
        /// <summary>
        /// Adds a custom entity binder to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which the binder is added.</param>
        /// <param name="insertPosition">The position in the collection where the binder is inserted.</param>
        public static void AddEntityBinder(this IServiceCollection services, int insertPosition)
        {
            services.AddControllers(options =>
            {
                //options.Filters.Add<ModelBindingExceptionFilter>();
            });
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(insertPosition, new EntityBinderProvider());
            });
        }
    }
}
