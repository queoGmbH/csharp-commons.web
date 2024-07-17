using System;

namespace Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// This attribute is used to describe which actions are available for a user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionDescriptionAttribute : Attribute
    {
        private readonly string[] _permissions;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="permissions">The permissions required for the action.</param>
        public ActionDescriptionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        /// <summary>
        /// Gets the permissions required for the action.
        /// </summary>
        public string[] Permissions { get { return _permissions; } }
    }
}