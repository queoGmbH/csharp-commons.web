namespace Queo.Commons.Web.Security
{
    /// <summary>
    /// Represents an authorization service.
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// Checks if the user has the specified permission.
        /// </summary>
        /// <param name="permission">The permission to check.</param>
        /// <returns>True if the user has the permission; otherwise, false.</returns>
        bool HasPermission(string permission);

        /// <summary>
        /// Checks if the user has the specified role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the user has the role; otherwise, false.</returns>
        bool HasRole(string role);
    }
}