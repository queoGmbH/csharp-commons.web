using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;

namespace Queo.Commons.Web.Security
{
    /// <summary>
    /// Represents a class responsible for creating security contexts.
    /// </summary>
    public class SecurityContextCreator
    {
        private readonly IEnumerable<IPermissionCalculator> _permissionCalculators;
        private readonly ISecurityContextFactory _securityContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityContextCreator"/> class.
        /// </summary>
        /// <param name="permissionCalculators">The permission calculators.</param>
        /// <param name="securityContextFactory">The security context factory.</param>
        public SecurityContextCreator(IEnumerable<IPermissionCalculator> permissionCalculators, ISecurityContextFactory securityContextFactory)
        {
            _permissionCalculators = permissionCalculators;
            _securityContextFactory = securityContextFactory;
        }

        /// <summary>
        /// Creates a security context based on the provided principal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The created security context.</returns>
        /// <exception cref="SecurityException">Thrown when the principal's identity does not have a name.</exception>
        public ISecurityContext Create(IPrincipal principal)
        {
            if (string.IsNullOrWhiteSpace(principal.Identity?.Name))
            {
                throw new SecurityException("Can't create security context for an identity without name.");
            }

            List<string> permissions = [];
            foreach (var permissionCalculator in _permissionCalculators)
            {
                IList<string> tempPermissions = permissionCalculator.CalculatePermissions(principal);
                permissions.AddRange(tempPermissions);
            }
            permissions = permissions.Distinct().ToList();

            var securityContext = _securityContextFactory.Create(principal, permissions);
            return securityContext;
        }

        /// <summary>
        /// Returns an empty <see cref="ISecurityContext"/> without roles and permissions.
        /// </summary>
        /// <returns>The empty security context.</returns>
        public ISecurityContext CreateEmpty()
        {
            return _securityContextFactory.CreateEmpty();
        }
    }
}
