using Queo.Commons.Web.Security.SecurityContextAccessor;

namespace Queo.Commons.Web.Security
{
    /// <summary>
    /// Provides methods to check whether the user is authorized to perform a certain operation.
    /// </summary>
    public class AuthorizeService : IAuthorizeService
    {
        private readonly ISecurityContext _securityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeService"/> class.
        /// </summary>
        /// <param name="securityContextAccessor">The security context accessor.</param>
        public AuthorizeService(ISecurityContextAccessor<ISecurityContext> securityContextAccessor)
        {
            _securityContext = securityContextAccessor.GetCurrent();
        }

        /// <summary>
        /// Checks whether the current user has the specified permission.
        /// </summary>
        /// <param name="permission">The permission to check.</param>
        /// <returns>True if the current user has the specified permission; otherwise, false.</returns>
        public bool HasPermission(string permission)
        {
            return _securityContext.Permissions.Contains(permission);
        }

        /// <summary>
        /// Checks whether the current user has the specified role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the current user has the specified role; otherwise, false.</returns>
        public bool HasRole(string role)
        {
            return _securityContext.HasRole(role);
        }
    }
}