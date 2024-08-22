using System.Collections.ObjectModel;

namespace Commons.Web.Security.Tests.RequiredImplementations
{
    /// <summary>
    /// Represents the security context for a user, containing information about their identity, roles, and permissions.
    /// </summary>
    public class SecurityContext : ISecurityContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityContext"/> class.
        /// </summary>
        public SecurityContext()
        {
            Id = Guid.NewGuid();
            Permissions = new ReadOnlyCollection<string>(new List<string>());
            Roles = new ReadOnlyCollection<Role>(new List<Role>());
            IdentityName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityContext"/> class with the specified identity name, roles, and permissions.
        /// </summary>
        /// <param name="identityName">The identity name.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="permissions">The permissions.</param>
        public SecurityContext(string identityName, ICollection<Role> roles, ICollection<string> permissions)
        {
            Id = Guid.NewGuid();
            IdentityName = identityName;
            Roles = new ReadOnlyCollection<Role>(roles.ToList());
            Permissions = new ReadOnlyCollection<string>(permissions.ToList());
        }

        /// <summary>
        /// Gets the unique identifier of the security context.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets the identity name associated with the security context.
        /// </summary>
        public string IdentityName { get; protected set; }

        /// <summary>
        /// Gets the roles associated with the security context.
        /// </summary>
        public ICollection<Role> Roles { get; protected set; }

        /// <summary>
        /// Gets the permissions associated with the security context.
        /// </summary>
        public ICollection<string> Permissions { get; protected set; }

        /// <summary>
        /// Determines whether the security context has the specified permission.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns><c>true</c> if the security context has the specified permission; otherwise, <c>false</c>.</returns>
        public bool HasPermission(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                return false;
            }
            return Permissions.Any(p => p.Equals(permissionName));
        }

        /// <summary>
        /// Determines whether the security context has the specified role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns><c>true</c> if the security context has the specified role; otherwise, <c>false</c>.</returns>
        public bool HasRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            return Roles.Any(p => p.Name.Equals(roleName));
        }
    }
}
