using System.Security.Principal;

namespace Commons.Web.Security
{
    /// <summary>
    /// Provides methods for manipulating the security contexts.
    /// Please do not use this interface directly in the functional context, but always use the accessor.
    /// </summary>
    public interface ISecurityContextHolder
    {
        /// <summary>
        /// Gets the security context for the principal.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        ISecurityContext GetSecurityContext(IPrincipal principal);

        /// <summary>
        /// Checks whether there is a SecurityContext for this principal.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        bool Has(IPrincipal principal);

        /// <summary>
        /// Adds a <see cref="ISecurityContext"/> to the holder.
        /// </summary>
        /// <param name="securityContext"></param>
        void Add(ISecurityContext securityContext);
    }
}
