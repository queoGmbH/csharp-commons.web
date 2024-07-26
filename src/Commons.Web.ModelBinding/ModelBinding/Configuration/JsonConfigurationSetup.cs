using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Queo.Commons.Persistence;

namespace Commons.Web.ModelBinding.Configuration
{
    public class JsonConfigurationSetup : IConfigureOptions<JsonOptions>
    {
        /// <summary>Initializes a new instance of the <see cref="JsonConfigurationSetup"/> class.</summary>
        public JsonConfigurationSetup()
        {
        }

        /// <summary>
        /// Invoked to configure a <see cref="JsonOptions"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="JsonOptions"/> instance to configure.</param>
        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(new JsonEntityConverter<Entity>());
            options.JsonSerializerOptions.Converters.Add(new JsonEntityListConverter<Entity>());
        }
    }
}