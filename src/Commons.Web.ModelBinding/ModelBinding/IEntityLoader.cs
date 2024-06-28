using System;

namespace Commons.Web.ModelBinding
{
    /// <summary>
    ///     Describes a loader that loads an entity based BusinessId
    /// </summary>
    public interface IEntityLoader<TEntity>
    {
        /// <summary>
        /// Returns the entity with the corresponding BusinessId
        /// </summary>
        /// <param name="businessId">BusinessId of the entity</param>
        /// <returns></returns>
        TEntity GetEntityByBusinessId(Guid businessId);
    }
}