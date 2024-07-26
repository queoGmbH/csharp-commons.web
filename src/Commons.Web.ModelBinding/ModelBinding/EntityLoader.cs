using System;
using Commons.Web.ModelBinding.ExceptionHandling;
using Queo.Commons.Persistence;
using Queo.Commons.Persistence.Exceptions;

namespace Commons.Web.ModelBinding
{
    public class EntityLoader<TEntity> : IEntityLoader<TEntity> where TEntity : Entity
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public EntityLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public TEntity GetEntityByBusinessId(Guid businessId)
        {
            IEntityDao<TEntity> dao = GetDao();
            try
            {
                return dao.GetByBusinessId(businessId);
            }
            catch (EntityNotFoundException ex)
            {
                throw new ModelBindingException(ex.Message, 404);
            }
        }

        /// <summary>
        /// Gets the DAO corresponding to the type of TEntity.
        /// </summary>
        /// <returns>The DAO corresponding to the type of TEntity.</returns>
        private IEntityDao<TEntity> GetDao()
        {
            // Get the full name of the entity class
            string entityTypeName = typeof(TEntity).Name;

            // Build the generic type using Type.MakeGenericType
            Type genericDao = typeof(IEntityDao<>).MakeGenericType(typeof(TEntity));

            IEntityDao<TEntity>? daoInstance = _serviceProvider.GetService(genericDao) as IEntityDao<TEntity>;

            if (daoInstance != null) return daoInstance;

            // If the DAO class was not found, throw an exception
            throw new ModelBindingException($"No DAO defined for entity type {entityTypeName}", 400);
        }
    }
}
