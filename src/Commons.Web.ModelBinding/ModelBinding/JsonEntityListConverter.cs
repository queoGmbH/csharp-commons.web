using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Queo.Commons.Persistence;

using Queo.Commons.Web.ModelBinding.ExceptionHandling;

namespace Queo.Commons.Web.ModelBinding
{
    /// <summary>
    /// Provides a <see cref="JsonConverter" /> that attempts to load a list of entities when they are only
    /// passed as a list of ids.
    /// </summary>
    public class JsonEntityListConverter<TEntity> : JsonConverter<IList<TEntity>> where TEntity : Entity
    {
        /// <summary>
        /// Reads a list of business IDs from the JSON reader, loads the corresponding entities, and returns them as a list.
        /// </summary>
        public override IList<TEntity> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IList<Guid>? businessIds = JsonSerializer.Deserialize<IList<Guid>>(ref reader, options);
            List<TEntity> resultList = [];
            if (businessIds == null)
            {
                return resultList;
            }
            else
            {
                var entityLoaderType = typeof(EntityLoader<>).MakeGenericType(typeToConvert);
                dynamic? _entityLoader = Activator.CreateInstance(entityLoaderType) ??
                    throw new ModelBindingException($"No entity loader defined for entity type {typeToConvert.Name}", 400);
                foreach (Guid businessId in businessIds)
                {
                    resultList.Add(_entityLoader.GetEntityByBusinessId(businessId));
                }
                return resultList;
            }
        }

        /// <summary>
        /// Throws a NotImplementedException because writing JSON is not supported by this converter.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, IList<TEntity> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}