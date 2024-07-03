﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Queo.Commons.Persistence;

using Commons.Web.ModelBinding.ExceptionHandling;

namespace Commons.Web.ModelBinding
{
    public class JsonEntityConverter<TEntity> : JsonConverter<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the JsonEntityConverter class.
        /// </summary>
        public JsonEntityConverter()
        {
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return IsBindableModelType(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="options">The calling options.</param>
        /// <returns>The object value.</returns>
        public override TEntity Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            Guid businessId = TryParseGuid(ref reader);
            TEntity entity = (TEntity)GetEntityByBusinessId(businessId, objectType);
            return entity;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The object value.</param>
        /// <param name="options">The calling options.</param>
        public override void Write(Utf8JsonWriter writer, TEntity value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartObject();

                foreach (var prop in value.GetType().GetProperties())
                {
                    writer.WritePropertyName(prop.Name);

                    var val = prop.GetValue(value);
                    if (val != null)
                    {
                        JsonSerializer.Serialize(writer, val, val.GetType(), options);
                    }
                    else
                    {
                        writer.WriteNullValue();
                    }
                }

                writer.WriteEndObject();
            }
        }

        private Guid TryParseGuid(ref Utf8JsonReader reader)
        {
            // Attempt to parse the string value as a Guid
            try
            {
                return reader.GetGuid();
            }
            // If parsing fails, throw a ModelBindingException with a detailed error message
            catch (Exception)
            {
                throw new ModelBindingException($" is not a valid Guid.", 400);
            }
        }

        /// <summary>
        /// Retrieves the entity by its BusinessId.
        /// </summary>
        /// <param name="businessId">The BusinessId of the entity.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>The entity.</returns>
        private object GetEntityByBusinessId(Guid businessId, Type entityType)
        {
            var entityLoaderType = typeof(EntityLoader<>).MakeGenericType(entityType);
            dynamic? _entityLoader = Activator.CreateInstance(entityLoaderType) ??
                throw new ModelBindingException($"No entity loader defined for entity type {entityType.Name}", 400);
            object entity = _entityLoader.GetEntityByBusinessId(businessId);
            return entity;
        }

        /// <summary>
        /// Determines whether the specified model type is bindable.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>true if the model type is bindable; otherwise, false.</returns>
        private bool IsBindableModelType(Type modelType)
        {
            return typeof(Entity).IsAssignableFrom(modelType);
        }
    }
}