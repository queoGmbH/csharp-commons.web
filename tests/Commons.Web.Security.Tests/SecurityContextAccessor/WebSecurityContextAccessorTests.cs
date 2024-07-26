using System.Security;
using System.Security.Claims;
using Commons.Web.Security.SecurityContextAccessor;
using Microsoft.AspNetCore.Http;

using Moq;
using NUnit.Framework;

namespace Commons.Web.Security.Tests.SecurityContextAccessor
{
    [TestFixture]
    public class WebSecurityContextAccessorTests
    {
        private Mock<ISecurityContextHolder> securityContextHolderMock;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;
        private WebSecurityContextAccessor webSecurityContextAccessor;

        [SetUp]
        public void Setup()
        {
            securityContextHolderMock = new Mock<ISecurityContextHolder>();
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            webSecurityContextAccessor = new WebSecurityContextAccessor(securityContextHolderMock.Object, httpContextAccessorMock.Object);
        }

        [Test]
        public void GetCurrent_WhenHttpContextNull_ThrowsSecurityException()
        {
            // Arrange
#pragma warning disable CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
#pragma warning restore CS8600 // Das NULL-Literal oder ein möglicher NULL-Wert wird in einen Non-Nullable-Typ konvertiert.

            // Act & Assert
            Assert.Throws<SecurityException>(() => webSecurityContextAccessor.GetCurrent());
        }

        [Test]
        public void GetCurrent_WhenPrincipalNotNull_ReturnsSecurityContext()
        {
            // Arrange
            var principal = new ClaimsPrincipal();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = principal });
            var expectedSecurityContext = new Mock<ISecurityContext>().Object;
            securityContextHolderMock.Setup(x => x.GetSecurityContext(principal)).Returns(expectedSecurityContext);

            // Act
            ISecurityContext result = webSecurityContextAccessor.GetCurrent();

            // Assert
            Assert.That(result, Is.EqualTo(expectedSecurityContext));
        }
    }
}
