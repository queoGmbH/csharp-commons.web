using NUnit.Framework;
using Moq;

using FluentAssertions;
using Queo.Commons.Web.ModelBinding.Tests.Domain;
using Queo.Commons.Web.ModelBinding.Tests.Persistence;

using Queo.Commons.Persistence;
using Queo.Commons.Web.ModelBinding.ExceptionHandling;

namespace Queo.Commons.Web.ModelBinding.Tests
{
    [TestFixture]
    public class EntityLoaderTest
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        public EntityLoaderTest()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();

            // Mocking the GetService method to return a TestEntityDao instance
            Type genericTestEntityDao = typeof(IEntityDao<>).MakeGenericType(typeof(TestEntity));
            _serviceProviderMock.Setup(mock => mock.GetService(genericTestEntityDao)).Returns(new TestEntityDao());
        }

        [Test]
        public void GetEntityByBusinessId_ExistingEntity_ReturnsEntity()
        {
            // GIVEN
            EntityLoader<TestEntity> _entityLoader = new EntityLoader<TestEntity>(_serviceProviderMock.Object);
            Guid testEntityBusinessId = Guid.Parse("1b8fe094-ff53-47ad-9e5d-c6690a13844a");

            // WHEN
            TestEntity testEntity = _entityLoader.GetEntityByBusinessId(testEntityBusinessId);

            // THEN
            testEntity.BusinessId.Should().Be(testEntityBusinessId);
            testEntity.Name.Should().Be("The First TestEntity");
            testEntity.Value.Should().Be(1);
        }

        [Test]
        public void GetEntityByBusinessId_NotExistingEntity_ThrowsException()
        {
            // GIVEN
            EntityLoader<TestEntity> _entityLoader = new EntityLoader<TestEntity>(_serviceProviderMock.Object);
            Guid testEntityBusinessId = Guid.Parse("1b8fe094-ff53-47ad-9e5d-c6690a13844b");


            // WHEN
            TestDelegate testDelegate = () => _entityLoader.GetEntityByBusinessId(testEntityBusinessId);

            // THEN
            Exception exception = Assert.Throws<ModelBindingException>(testDelegate);
            exception.Message.Should().Be($"Entity with businessId {testEntityBusinessId} not found.");
        }

        [Test]
        public void GetEntityByBusinessId_NoDaoRegistered_ThrowsException()
        {
            // GIVEN
            EntityLoader<FakeOrder> _entityLoader = new EntityLoader<FakeOrder>(_serviceProviderMock.Object);
            Guid testEntityBusinessId = Guid.Parse("1b8fe094-ff53-47ad-9e5d-c6690a13844a");

            // WHEN
            TestDelegate testDelegate = () => _entityLoader.GetEntityByBusinessId(testEntityBusinessId);

            // THEN
            Exception exception = Assert.Throws<ModelBindingException>(testDelegate);
            exception.Message.Should().Be("No DAO defined for entity type FakeOrder");
        }
    }
}
