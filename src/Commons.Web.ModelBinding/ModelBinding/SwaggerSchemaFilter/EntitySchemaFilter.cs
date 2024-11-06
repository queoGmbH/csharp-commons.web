using System;
using NJsonSchema;
using NJsonSchema.Generation;

using Queo.Commons.Persistence;

namespace Queo.Commons.Web.ModelBinding.SwaggerSchemaFilter
{
    public class EntitySchemaFilter : ISchemaProcessor
    {
        /// <summary>
        /// Processes the schema using the provided context.
        /// </summary>
        /// <param name="context">The schema processor context.</param>
        public void Process(SchemaProcessorContext context)
        {
            // Check if the context type is assignable from Entity
            if (typeof(Entity).IsAssignableFrom(context.ContextualType))
            {
                // Set the schema type to string
                context.Schema.Type = JsonObjectType.String;

                // Set the schema format to uuid
                context.Schema.Format = JsonFormatStrings.Guid;

                // Set an example for the schema using a new GUID
                context.Schema.Example = Guid.NewGuid().ToString();
            }
        }
    }


}
