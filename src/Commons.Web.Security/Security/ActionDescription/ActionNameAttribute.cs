using System;

namespace Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// Use this attribute to specify the name of the permission that is represented by the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionNameAttribute : Attribute
    {
        private readonly string _actionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionNameAttribute"/> class.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        public ActionNameAttribute(string actionName)
        {
            _actionName = actionName;
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        public string ActionName { get { return _actionName; } }
    }
}