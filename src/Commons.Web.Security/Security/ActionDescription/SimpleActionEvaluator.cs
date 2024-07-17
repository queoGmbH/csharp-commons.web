namespace Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// Evaluates a action that is simple checked by a permission.
    /// </summary>
    internal class SimpleActionEvaluator : IActionEvaluator
    {
        private readonly string _neededPermission;
        private readonly string _actionDescription;
        private readonly ISecurityContext _securityContext;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="actionDescription"></param>
        /// <param name="securityContext"></param>
        public SimpleActionEvaluator(string actionDescription, ISecurityContext securityContext)
        {
            _neededPermission = actionDescription;
            _actionDescription = actionDescription;
            _securityContext = securityContext;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="identifierExpression"></param>
        /// <param name="parameterValue"></param>
        /// <param name="securityContext"></param>
        public SimpleActionEvaluator(IdentifierExpression identifierExpression, string parameterValue, ISecurityContext securityContext)
        {
            _neededPermission = identifierExpression.BuildConcreteExpression(parameterValue);
            _actionDescription = identifierExpression.BaseExpression;
            _securityContext = securityContext;
        }

        /// <summary>
        /// The permission that is needed to execute the action.
        /// </summary>
        public string NeededPermission { get { return _neededPermission; } }

        /// <summary>
        /// The description of the action.
        /// </summary>
        public string ActionDescription { get { return _actionDescription; } }

        /// <inheritdoc />
        public EvaluationResult Evaluate()
        {
            bool canExecute = _securityContext.HasPermission(_actionDescription);
            return new EvaluationResult(_actionDescription, canExecute);
        }
    }
}