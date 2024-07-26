using Commons.Web.Security;
using Commons.Web.Security.ActionDescription;
using Commons.Web.Security.SecurityContextAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Moq;
using NUnit.Framework;

namespace Commons.Web.Security.Tests.ActionDescription
{


    [TestFixture]
    public class IdentifierActionEvaluatorTests
    {
        private ActionExecutingContext _context;

        [SetUp]
        public void Setup()
        {
            Mock<ISecurityContextAccessor> securityContextAccessorMock = new Mock<ISecurityContextAccessor>();
            securityContextAccessorMock.Setup(x => x.GetCurrent()).Returns(new Mock<ISecurityContext>().Object);
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(ISecurityContextAccessor))).Returns(securityContextAccessorMock.Object);

            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.RequestServices).Returns(serviceProviderMock.Object);

            _context = new ActionExecutingContext(new ActionContext(httpContextMock.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                new Mock<object>().Object);
        }

        [Test]
        public void GetParameterValue_WithValidParameter_ReturnsCorrectValue()
        {
            // Arrange
            IdentifierExpression identifierExpression = new IdentifierExpression("create_proposed_change_{id}");
            IdentifierEvaluatorBuilder builder = new IdentifierEvaluatorBuilder("create_proposed_change_{id}");
            _context.RouteData.Values.Add("id", "TestValue");

            // Act
            var evaluator = (SimpleActionEvaluator)builder.Build(_context);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(evaluator.NeededPermission, Is.EqualTo("create_proposed_change_TestValue"));
                Assert.That(evaluator.ActionDescription, Is.EqualTo("create_proposed_change"));
            });
        }

        [Test]
        public void GetParameterValue_WithMissingParameter_ThrowsException()
        {
            // Arrange
            string actionDescriptor = "edit_entity_{id}";
            IdentifierExpression identifierExpression = new IdentifierExpression(actionDescriptor);
            IdentifierEvaluatorBuilder builder = new IdentifierEvaluatorBuilder(actionDescriptor);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => builder.Build(_context));
        }
    }
}
