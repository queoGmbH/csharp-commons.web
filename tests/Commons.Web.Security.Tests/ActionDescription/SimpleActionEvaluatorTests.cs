using Commons.Web.Security;
using Commons.Web.Security.ActionDescription;

using Moq;
using NUnit.Framework;

namespace Commons.Web.Security.Tests.ActionDescription
{
    [TestFixture]
    public class SimpleActionEvaluatorTests
    {
        private Mock<ISecurityContext> _securityContextMock;
        private SimpleActionEvaluator _actionEvaluator;

        [SetUp]
        public void Setup()
        {
            _securityContextMock = new Mock<ISecurityContext>();
            _actionEvaluator = new SimpleActionEvaluator("can_create_proposed_change", _securityContextMock.Object);
        }

        [Test]
        public void Evaluate_WithPermission_ReturnsEvaluationResultWithCanExecuteTrue()
        {
            // Arrange
            _securityContextMock.Setup(x => x.HasPermission("can_create_proposed_change")).Returns(true);

            // Act
            var result = _actionEvaluator.Evaluate();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo("can_create_proposed_change"));
                Assert.That(result.CanExecute, Is.True);
            });
        }

        [Test]
        public void Evaluate_WithoutPermission_ReturnsEvaluationResultWithCanExecuteFalse()
        {
            // Arrange
            _securityContextMock.Setup(x => x.HasPermission("can_create_proposed_change")).Returns(false);

            // Act
            var result = _actionEvaluator.Evaluate();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo("can_create_proposed_change"));
                Assert.That(result.CanExecute, Is.False);
            });
        }
    }
}