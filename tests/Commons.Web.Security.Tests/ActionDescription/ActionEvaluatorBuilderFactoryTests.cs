using Commons.Web.Security.ActionDescription;

using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Commons.Web.Security.Tests.ActionDescription
{
    [TestFixture]
    public class ActionEvaluatorBuilderFactoryTests
    {
        [Test]
        public void GetBuilder_WhenActionDescriptionContainsBusinessId_ReturnsIdentifierEvaluatorBuilder()
        {
            // Arrange
            var actionDescription = "edit_document_{identifier}";

            // Act
            var result = new ActionEvaluatorBuilderFactory(new NullLogger<ActionEvaluatorBuilderFactory>()).GetBuilder(actionDescription);

            // Assert
            Assert.That(result, Is.InstanceOf<IdentifierEvaluatorBuilder>());
        }

        [Test]
        public void GetBuilder_WhenActionDescriptionContainsMethod_ReturnsIdentifierEvaluatorBuilder()
        {
            // Arrange
            var actionDescription = "CanEditDocument(#document, #currentUser)";

            // Act
            var result = new ActionEvaluatorBuilderFactory(new NullLogger<ActionEvaluatorBuilderFactory>()).GetBuilder(actionDescription);

            // Assert
            Assert.That(result, Is.InstanceOf<MethodEvaluatorBuilder>());
        }

        [Test]
        public void GetBuilder_WhenActionDescriptionContainsNeitherBusinessIdNorParentheses_ReturnsSimpleEvaluatorBuilder()
        {
            // Arrange
            var actionDescription = "edit_document";

            // Act
            var result = new ActionEvaluatorBuilderFactory(new NullLogger<ActionEvaluatorBuilderFactory>()).GetBuilder(actionDescription);

            // Assert
            Assert.That(result, Is.InstanceOf<SimpleEvaluatorBuilder>());
        }

        [Test]
        public void GetBuilder_WhenActionDescriptionCanNotBeEvaluated_ThrowsInvalidOperationException()
        {
            // Arrange
            var actionDescription = "anytext";

            // Act
            void Act() => new ActionEvaluatorBuilderFactory(new NullLogger<ActionEvaluatorBuilderFactory>()).GetBuilder(actionDescription);

            // Assert
            Assert.That(Act, Throws.InvalidOperationException);
        }
    }
}
