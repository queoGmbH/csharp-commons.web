using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Commons.Web.ModelBinding.Configuration
{
    public static class JsonModelBindingConfiguration
    {
        /// <summary>
        /// Configures the JSON ModelBinder for use. This includes setting up the HttpContextAccessor,
        /// configuring the JsonOptions, and setting the JsonSerializerOptions to use CamelCase property naming
        /// and to write indented JSON.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the JSON Model Converter to.</param>
        public static void AddJsonModelConverter(this IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<JsonOptions>, JsonConfigurationSetup>();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
            });
        }
    }
}