using System.Collections.Generic;

namespace Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// Encapsulates the original result of an action and additional data.
    /// </summary>
    public class ActionDecoratorResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDecoratorResult"/> class.
        /// </summary>
        /// <param name="result">The original result of the action.</param>
        /// <param name="additionalData">Additional data describing the possible actions that a user can perform or not.</param>
        public ActionDecoratorResult(object? result, Dictionary<string, bool> additionalData)
        {
            Result = result;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// Gets the original result of the action.
        /// </summary>
        public object? Result { get; }

        /// <summary>
        /// Gets additional data describing the possible actions that a user can perform or not.
        /// </summary>
        public Dictionary<string, bool> AdditionalData { get; }
    }
}