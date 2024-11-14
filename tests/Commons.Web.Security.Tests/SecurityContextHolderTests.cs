using System.Security;
using System.Security.Principal;
using Queo.Commons.Web.Security.Tests.RequiredImplementations;
using NUnit.Framework;



namespace Queo.Commons.Web.Security.Tests
{
    public class SecurityContextHolderTests
    {
        private SecurityContextHolder _securityContextHolder;

        [SetUp]
        public void SetUp()
        {
            _securityContextHolder = new SecurityContextHolder();
        }

        [Test]
        public void GetSecurityContext_ShouldThrowSecurityException_WhenIdentityNameIsEmpty()
        {
            // Arrange
            var user = new GenericPrincipal(new GenericIdentity(""), Array.Empty<string>());

            // Act & Assert
            Assert.Throws<SecurityException>(() => _securityContextHolder.GetSecurityContext(user));
        }

        [Test]
        public void GetSecurityContext_ShouldThrowInvalidOperationException_WhenSecurityContextIsNotFound()
        {
            // Arrange
            var user = new GenericPrincipal(new GenericIdentity("nonexistentuser"), Array.Empty<string>());

            // Act & Assert
            Assert.Throws<SecurityException>(() => _securityContextHolder.GetSecurityContext(user));
        }

        [Test]
        public void GetSecurityContext_ShouldReturnSecurityContext_WhenSecurityContextIsFound()
        {
            // Arrange
            var user = new GenericPrincipal(new GenericIdentity("existinguser"), Array.Empty<string>());
            var securityContext = new SecurityContext("existinguser", new List<Role>(), new List<string>());

            _securityContextHolder.Add(securityContext);

            // Act
            var result = _securityContextHolder.GetSecurityContext(user);

            // Assert
            Assert.That(result, Is.EqualTo(securityContext));
        }

        [Test]
        public void Has_ShouldThrowInvalidOperationException_WhenIdentityNameIsEmpty()
        {
            // Arrange
            var principal = new GenericPrincipal(new GenericIdentity(""), Array.Empty<string>());

            // Act & Assert
            Assert.Throws<SecurityException>(() => _securityContextHolder.Has(principal));
        }

        [Test]
        public void Has_ShouldReturnFalse_WhenSecurityContextIsNotFound()
        {
            // Arrange
            var principal = new GenericPrincipal(new GenericIdentity("nonexistentuser"), Array.Empty<string>());

            // Act
            var result = _securityContextHolder.Has(principal);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Has_ShouldReturnTrue_WhenSecurityContextIsFound()
        {
            // Arrange
            var principal = new GenericPrincipal(new GenericIdentity("existinguser"), Array.Empty<string>());
            var securityContext = new SecurityContext("existinguser", new List<Role>(), new List<string>());

            _securityContextHolder.Add(securityContext);

            // Act
            var result = _securityContextHolder.Has(principal);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Add_ShouldAddSecurityContextToCache()
        {
            // Arrange
            var securityContext = new SecurityContext("existinguser", new List<Role>(), new List<string>());

            // Act
            _securityContextHolder.Add(securityContext);

            // Assert
            Assert.That(_securityContextHolder.Has(new GenericPrincipal(new GenericIdentity("existinguser"), Array.Empty<string>())), Is.True);
        }
    }
}
