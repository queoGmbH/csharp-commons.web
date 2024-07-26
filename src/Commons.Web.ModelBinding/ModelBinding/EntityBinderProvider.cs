using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

using Queo.Commons.Checks;
using Queo.Commons.Persistence;

namespace Commons.Web.ModelBinding
{
    /// <summary>
    /// Provides a model binder for Entity types.
    /// </summary>
    public class EntityBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Gets a binder that can bind models of the given type, if one is available.
        /// </summary>
        /// <param name="context">The model binder provider context.</param>
        /// <returns>A model binder, or null if none is available for the given type.</returns>
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Require.NotNull(context, nameof(context));
            Type modelType = context.Metadata.ModelType;
            if (typeof(Entity).IsAssignableFrom(modelType))
            {
                // Get the type of the EntityBinder<> class
                Type genericClass = typeof(EntityBinder<>);

                // Make it specific by adding the model type
                Type constructedClass = genericClass.MakeGenericType(context.Metadata.ModelType);

                // Return a new BinderTypeModelBinder with the constructed class
                return new BinderTypeModelBinder(constructedClass);
            }
            return null;
        }
    }
}
