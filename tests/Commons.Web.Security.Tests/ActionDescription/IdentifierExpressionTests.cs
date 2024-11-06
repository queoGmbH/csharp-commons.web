using Queo.Commons.Web.Security.ActionDescription;
using NUnit.Framework;

namespace Queo.Commons.Web.Security.Tests.ActionDescription
{

    [TestFixture]
    public class IdentifierExpressionTests
    {
        [Test]
        public void BuildConcreteExpression_WithValidIdentifier_ReturnsCorrectExpression()
        {
            // Arrange
            string expression = "create_proposed_change_{id}";
            string identifier = "TestValue";
            IdentifierExpression identifierExpression = new IdentifierExpression(expression);

            // Act
            string concreteExpression = identifierExpression.BuildConcreteExpression(identifier);

            // Assert
            Assert.That(concreteExpression, Is.EqualTo("create_proposed_change_TestValue"));
        }

        [Test]
        public void GetParameterName_WithValidExpression_ReturnsCorrectParameterName()
        {
            // Arrange
            string expression = "create_proposed_change_{id}";
            IdentifierExpression identifierExpression = new IdentifierExpression(expression);

            // Act
            string parameterName = identifierExpression.ParameterName;

            // Assert
            Assert.That(parameterName, Is.EqualTo("id"));
        }

        [Test]
        public void GetBaseExpression_WithValidExpression_ReturnsCorrectBaseExpression()
        {
            // Arrange
            string expression = "create_proposed_change_{id}";
            IdentifierExpression identifierExpression = new IdentifierExpression(expression);

            // Act
            string baseExpression = identifierExpression.BaseExpression;

            // Assert
            Assert.That(baseExpression, Is.EqualTo("create_proposed_change"));
        }
    }
}
