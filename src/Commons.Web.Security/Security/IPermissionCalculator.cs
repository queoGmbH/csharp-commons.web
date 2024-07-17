using System.Collections.Generic;
using System.Security.Principal;

namespace Commons.Web.Security
{
    /// <summary>
    /// Represents a permission calculator that calculates a set of permissions for a given principal.
    /// </summary>
    public interface IPermissionCalculator
    {
        /// <summary>
        /// Calculates a set of permissions for the principal.
        /// </summary>
        /// <param name="principal">The principal for which to calculate the permissions.</param>
        /// <returns>A list of granted permissions.</returns>
        IList<string> CalculatePermissions(IPrincipal principal);
    }
}
