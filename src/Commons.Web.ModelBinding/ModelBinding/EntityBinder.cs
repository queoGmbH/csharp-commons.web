using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Queo.Commons.Checks;
using Queo.Commons.Persistence;

using Commons.Web.ModelBinding.ExceptionHandling;

namespace Commons.Web.ModelBinding
{
    /// <summary>
    /// This class provides methods for binding models in a way that supports 
    /// different types of entities and their corresponding DAOs.
    /// </summary>
    public class EntityBinder<TEntity> : IModelBinder where TEntity : Entity
    {
        /// <summary>
        /// Asynchronously binds the model for the given context.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>A task representing the model binding operation.</returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Check if the binding context is null
            Require.NotNull(bindingContext, nameof(bindingContext));

            string modelName = GetModelName(bindingContext);
            string? value = GetModelValue(bindingContext, modelName);

            // Create a new model binding result and set it to failed
            if (string.IsNullOrEmpty(value))
            {
                throw new ModelBindingException("Value must not be null or empty.", 400);
            }
            Guid businessId = TryParseGuid(value);
            try
            {
                IEntityLoader<TEntity> entityLoader = new EntityLoader<TEntity>(bindingContext.HttpContext.RequestServices);
                Entity model = entityLoader.GetEntityByBusinessId(businessId);
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }
            catch (Exception ex) when (ex is not ModelBindingException)
            {
                throw new ModelBindingException(ex.Message, 400);
            }
        }

        /// <summary>
        /// Gets the model name from the given binding context.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The model name.</returns>
        private string GetModelName(ModelBindingContext bindingContext)
        {
            return bindingContext.ModelName;
        }

        /// <summary>
        /// Gets the model value from the given binding context and model name.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="modelName">The model name.</param>
        /// <returns>The model value.</returns>
        private string? GetModelValue(ModelBindingContext bindingContext, string modelName)
        {
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return null;
            }
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            return valueProviderResult.FirstValue;
        }

        /// <summary>
        /// Tries to parse the given value as a Guid.
        /// </summary>
        /// <param name="value">The value to parse.</param
        /// <returns>The Guid value if it is valid.</returns>
        private Guid TryParseGuid(string value)
        {
            try
            {
                return Guid.Parse(value);
            }
            catch
            {
                throw new ModelBindingException("Id must be a Guid.", 400);
            }
        }
    }
}

