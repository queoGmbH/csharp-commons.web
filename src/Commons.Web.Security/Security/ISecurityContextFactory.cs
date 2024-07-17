using System.Collections.Generic;
using System.Security.Principal;

namespace Commons.Web.Security 
{
    /// <summary>
    /// Represents a factory for creating security contexts.
    /// </summary>
    public interface ISecurityContextFactory
    {
        /// <summary>
        /// Creates a security context with the specified principal and permissions.
        /// </summary>
        /// <param name="principal">The principal associated with the security context.</param>
        /// <param name="permissions">The list of permissions associated with the security context.</param>
        /// <returns>The created security context.</returns>
        ISecurityContext Create(IPrincipal principal, IList<string> permissions);

        /// <summary>
        /// Creates an empty security context.
        /// </summary>
        /// <returns>The created security context.</returns>
        ISecurityContext CreateEmpty();
    }
}