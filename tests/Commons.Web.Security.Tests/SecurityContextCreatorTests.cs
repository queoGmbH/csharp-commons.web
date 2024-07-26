using System.Security;
using System.Security.Principal;
using Moq;
using NUnit.Framework;

namespace Commons.Web.Security.Tests
{
    [TestFixture]
    public class SecurityContextCreatorTests
    {
        private Mock<IPermissionCalculator> _permissionCalculatorMock;
        private Mock<ISecurityContextFactory> _securityContextFactoryMock;
        private SecurityContextCreator _securityContextCreator;

        [SetUp]
        public void SetUp()
        {
            _permissionCalculatorMock = new Mock<IPermissionCalculator>();
            _securityContextFactoryMock = new Mock<ISecurityContextFactory>();
            _securityContextCreator = new SecurityContextCreator(new List<IPermissionCalculator> { _permissionCalculatorMock.Object }, _securityContextFactoryMock.Object);
        }

        [Test]
        public void Create_WithValidPrincipal_ReturnsSecurityContext()
        {
            // Arrange
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(p => p.Identity.Name).Returns("JohnDoe");

            var permissions = new List<string> { "Permission1", "Permission2" };
            _permissionCalculatorMock.Setup(p => p.CalculatePermissions(principalMock.Object)).Returns(permissions);

            var securityContextMock = new Mock<ISecurityContext>();
            _securityContextFactoryMock.Setup(f => f.Create(principalMock.Object, permissions)).Returns(securityContextMock.Object);

            // Act
            var result = _securityContextCreator.Create(principalMock.Object);

            // Assert
            Assert.AreEqual(securityContextMock.Object, result);
        }

        [Test]
        public void Create_WithPrincipalWithoutName_ThrowsSecurityException()
        {
            // Arrange
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(p => p.Identity.Name).Returns("");

            // Act & Assert
            Assert.Throws<SecurityException>(() => _securityContextCreator.Create(principalMock.Object));
        }

        [Test]
        public void CreateEmpty_ReturnsEmptySecurityContext()
        {
            // Arrange
            var emptySecurityContextMock = new Mock<ISecurityContext>();
            _securityContextFactoryMock.Setup(f => f.CreateEmpty()).Returns(emptySecurityContextMock.Object);

            // Act
            var result = _securityContextCreator.CreateEmpty();

            // Assert
            Assert.AreEqual(emptySecurityContextMock.Object, result);
        }
    }
}
