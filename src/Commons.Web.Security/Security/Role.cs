using System.Collections.Generic;

namespace Commons.Web.Security
{
    /// <summary>
    /// Represents a role that can be assigned to users and represents a collection of permissions for these users.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">The name of the role.</param>
        /// <param name="description">The description of the role.</param>
        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the permissions associated with the role.
        /// </summary>
        public ICollection<string> Permissions { get; init; } = new List<string>();
    }
}
