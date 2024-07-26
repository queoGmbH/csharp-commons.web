using System;
using System.Collections.Generic;
using NJsonSchema;
using NJsonSchema.Generation;

using Queo.Commons.Persistence;

namespace Commons.Web.ModelBinding.SwaggerSchemaFilter
{
    /// <summary>
    /// This class is used to apply a schema filter to Entity lists in the Swagger documentation.
    /// </summary>
    public class EntityListSchemaFilter : ISchemaProcessor
    {
        /// <summary>
        /// Applies the schema filter to the provided schema and context.
        /// </summary>
        /// <param name="schema">The OpenApiSchema to which the filter is applied.</param>
        /// <param name="context">The SchemaProcessorContext that provides the context for the filter.</param>
        public void Process(SchemaProcessorContext context)
        {
            // Check if the context type is a generic type and if it is a list of entities
            if (context.Type.IsGenericType &&
                context.Type.GetGenericTypeDefinition() == typeof(List<>) &&
                typeof(Entity).IsAssignableFrom(context.Type.GetGenericArguments()[0]))
            {
                // Set the schema type to array and define the items in the array
                context.Schema.Type = JsonObjectType.Array;
                context.Schema.Item = new JsonSchema
                {
                    Type = JsonObjectType.String,
                    Format = JsonFormatStrings.Guid,
                    // Provide an example of the UUID format
                    Example = Guid.NewGuid().ToString()
                };
            }
        }
    }

}