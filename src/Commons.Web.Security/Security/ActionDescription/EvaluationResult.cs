namespace Queo.Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// The result of an evaluation of whether the user can perform an action.
    /// </summary>
    public class EvaluationResult
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="canExecute"></param>
        public EvaluationResult(string name, bool canExecute)
        {
            Name = name;
            CanExecute = canExecute;
        }

        /// <summary>
        /// The name of the action for which it was evaluated whether the user can perform it.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Can the user perform the action or not.
        /// </summary>
        public bool CanExecute { get; }
    }
}