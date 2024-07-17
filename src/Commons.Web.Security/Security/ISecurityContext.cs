using System.Collections.Generic;

namespace Commons.Web.Security
{
    /// <summary>
    /// Interface for the security context
    /// </summary>
    public interface ISecurityContext
    {
        /// <summary>
        /// Returns a unique identifier for the user.
        /// </summary>
        string IdentityName { get; }

        /// <summary>
        /// Gets the roles associated with the user.
        /// </summary>
        ICollection<Role> Roles { get; }

        /// <summary>
        /// Gets the permissions associated with the user.
        /// </summary>
        ICollection<string> Permissions { get; }

        /// <summary>
        /// Returns true if the user has the expected permission, otherwise false.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns>True if the user has the expected permission, otherwise false.</returns>
        bool HasPermission(string permissionName);

        /// <summary>
        /// Returns true if the user has the role, otherwise false.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        bool HasRole(string roleName);
    }
}
