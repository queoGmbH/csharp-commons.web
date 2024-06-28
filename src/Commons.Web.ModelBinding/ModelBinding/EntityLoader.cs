using System;
using Commons.Web.ModelBinding.ExceptionHandling;
using Queo.Commons.Persistence;
using Queo.Commons.Persistence.Exceptions;

namespace Commons.Web.ModelBinding
{
    public class EntityLoader<TEntity> : IEntityLoader<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Gets the entity with the given business ID.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
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

            // Create the name of the DAO class
            string daoClassName = $"{entityTypeName}Dao";

            // Search all loaded assemblies for the DAO class
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name == daoClassName && typeof(IEntityDao<TEntity>).IsAssignableFrom(type))
                    {
                        // Create an instance of the DAO class
                        object daoInstance = Activator.CreateInstance(type)!;

                        // Return the DAO instance
                        return (IEntityDao<TEntity>)daoInstance;
                    }
                }
            }
            // If the DAO class was not found, throw an exception
            throw new ModelBindingException($"No DAO defined for entity type {entityTypeName}", 400);
        }
    }
}
