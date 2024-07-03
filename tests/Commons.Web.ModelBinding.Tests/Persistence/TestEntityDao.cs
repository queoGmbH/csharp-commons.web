using Queo.Commons.Persistence.Generic;

using Commons.Web.ModelBinding.Tests.Domain;
using Queo.Commons.Persistence.Exceptions;

namespace Commons.Web.ModelBinding.Tests.Persistence
{

    public class TestEntityDao : ITestEntityDao
    {
        private readonly List<TestEntity> _entities = [
            new TestEntity(Guid.Parse("1b8fe094-ff53-47ad-9e5d-c6690a13844a"), "The First TestEntity", 1),
            new TestEntity(Guid.Parse("d9ef4cb1-890d-42c3-8828-86cff54c9adc"), "The Second TestEntity", 2),
        ];

        ///<inheritdoc />
        public TestEntityDao()
        {
        }

        public TestEntity GetByBusinessId(Guid businessId)
        {
            TestEntity? entity = _entities.FirstOrDefault(e => e.BusinessId == businessId);
            if (entity != null)
            {
                return entity;
            }
            throw new EntityNotFoundException($"Entity with businessId {businessId} not found.");
        }

        TestEntity IGenericDao<TestEntity, int>.Add(TestEntity entity)
        {
            throw new NotImplementedException();
        }

        IList<TestEntity> IGenericDao<TestEntity, int>.Add(IList<TestEntity> entities)
        {
            throw new NotImplementedException();
        }

        Task<TestEntity> IGenericDao<TestEntity, int>.AddAsync(TestEntity entity)
        {
            throw new NotImplementedException();
        }

        Task<IList<TestEntity>> IGenericDao<TestEntity, int>.AddAsync(IList<TestEntity> entities)
        {
            throw new NotImplementedException();
        }

        void IGenericDao<TestEntity, int>.Clear()
        {
            throw new NotImplementedException();
        }

        void IGenericDao<TestEntity, int>.Delete(TestEntity entity)
        {
            throw new NotImplementedException();
        }

        bool IGenericDao<TestEntity, int>.Exists(int primaryKey)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericDao<TestEntity, int>.ExistsAsync(int primaryKey)
        {
            throw new NotImplementedException();
        }

        IList<TestEntity> IGenericDao<TestEntity, int>.FindAll()
        {
            throw new NotImplementedException();
        }

        Task<IList<TestEntity>> IGenericDao<TestEntity, int>.FindAllAsync()
        {
            throw new NotImplementedException();
        }

        IList<TestEntity> IEntityDao<TestEntity, int>.FindByBusinessIds(IList<Guid> businessIds)
        {
            throw new NotImplementedException();
        }

        Task<IList<TestEntity>> IEntityDao<TestEntity, int>.FindByBusinessIdsAsync(IList<Guid> businessIds)
        {
            throw new NotImplementedException();
        }

        IList<TestEntity> IGenericDao<TestEntity, int>.FindByIds(int[] ids)
        {
            throw new NotImplementedException();
        }

        Task<IList<TestEntity>> IGenericDao<TestEntity, int>.FindByIdsAsync(int[] ids)
        {
            throw new NotImplementedException();
        }

        void IGenericDao<TestEntity, int>.Flush()
        {
            throw new NotImplementedException();
        }

        Task IGenericDao<TestEntity, int>.FlushAsync()
        {
            throw new NotImplementedException();
        }

        TestEntity IGenericDao<TestEntity, int>.Get(int primaryKey)
        {
            throw new NotImplementedException();
        }

        IList<TestEntity> IGenericDao<TestEntity, int>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<IList<TestEntity>> IGenericDao<TestEntity, int>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<TestEntity> IGenericDao<TestEntity, int>.GetAsync(int primaryKey)
        {
            throw new NotImplementedException();
        }

        Task<TestEntity> IEntityDao<TestEntity, int>.GetByBusinessIdAsync(Guid businessId)
        {
            throw new NotImplementedException();
        }

        long IGenericDao<TestEntity, int>.GetCount()
        {
            throw new NotImplementedException();
        }

        Task<long> IGenericDao<TestEntity, int>.GetCountAsync()
        {
            throw new NotImplementedException();
        }

        Task<TestEntity> ITestEntityDao.GetEntityByBusinessId(Guid businessId)
        {
            throw new NotImplementedException();
        }

        TestEntity IGenericDao<TestEntity, int>.Save(TestEntity entity)
        {
            throw new NotImplementedException();
        }

        IList<TestEntity> IGenericDao<TestEntity, int>.Save(IList<TestEntity> entities)
        {
            throw new NotImplementedException();
        }

        Task<TestEntity> IGenericDao<TestEntity, int>.SaveAsync(TestEntity entity)
        {
            throw new NotImplementedException();
        }

        Task<IList<TestEntity>> IGenericDao<TestEntity, int>.SaveAsync(IList<TestEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
