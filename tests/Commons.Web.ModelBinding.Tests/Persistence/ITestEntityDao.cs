using Queo.Commons.Persistence;

using Commons.Web.ModelBinding.Tests.Domain;

namespace Commons.Web.ModelBinding.Tests.Persistence
{
    public interface ITestEntityDao : IEntityDao<TestEntity>
    {
        /// <summary>
        /// Gets the entity by the given business ID.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<TestEntity> GetEntityByBusinessId(Guid businessId);
    }
}
