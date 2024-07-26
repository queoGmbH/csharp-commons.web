using Queo.Commons.Persistence;

namespace Commons.Web.ModelBinding.Tests.Domain
{
    /// <summary>
    /// Represents a test entity for testing purposes.
    /// </summary>
    public class TestEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntity"/> class.
        /// </summary>
        /// <param name="businessId">The business ID of the entity.</param>
        /// <param name="name">The name of the entity.</param>
        /// <param name="value">The value of the entity.</param>
        public TestEntity(Guid businessId, string name, int value)
        {
            BusinessId = businessId;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the entity.
        /// </summary>
        public int Value { get; set; }
    }
}
