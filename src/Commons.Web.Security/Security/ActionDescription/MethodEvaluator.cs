using System.Linq;
using System.Reflection;

using Queo.Commons.Web.Security.MethodAuthorize;

namespace Queo.Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// This evaluator can determine an action's execution based on a method call.
    /// </summary>
    internal class MethodEvaluator : IActionEvaluator
    {
        private readonly MethodInfo _matchingMethod;
        private readonly MethodInformation _methodInformation;
        private readonly object _methodSecurityRoot;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="matchingMethod">The method to be evaluated.</param>
        /// <param name="methodInformation">The information about the method.</param>
        /// <param name="methodSecurityRoot">The root object for method security.</param>
        public MethodEvaluator(MethodInfo matchingMethod, MethodInformation methodInformation, object methodSecurityRoot)
        {
            _matchingMethod = matchingMethod;
            _methodInformation = methodInformation;
            _methodSecurityRoot = methodSecurityRoot;
        }

        /// <inheritdoc />
        public EvaluationResult Evaluate()
        {
            string actionName = GetActionName();
            // We call the method to check if the action can be executed. If the result is null, which shouldn't happen, we assume false.
            bool canExecute = (bool)(_matchingMethod.Invoke(_methodSecurityRoot, _methodInformation.MethodParameters.Select(x => x.ParameterValue).ToArray()) ?? false);
            EvaluationResult evaluationResult = new EvaluationResult(actionName, canExecute);
            return evaluationResult;
        }
        /// <summary>
        /// Gets the action name for the method.
        /// </summary>
        public string ActionName { get { return GetActionName(); } }

        /// <summary>
        /// Gets the action name for the method.
        /// </summary>
        /// <returns>The action name.</returns>
        private string GetActionName()
        {
            ActionNameAttribute? actionNameAttribute = _matchingMethod.GetCustomAttribute<ActionNameAttribute>();
            if (actionNameAttribute != null)
            {
                return actionNameAttribute.ActionName;
            }
            else
            {
                return _matchingMethod.Name;
            }
        }
    }
}