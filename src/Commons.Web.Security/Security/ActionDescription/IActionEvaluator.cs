namespace Queo.Commons.Web.Security.ActionDescription
{
    /// <summary>
    /// Interface for types that can evaluate whether an action can be executed.
    /// </summary>
    public interface IActionEvaluator
    {
        /// <summary>
        /// Evaluates whether an action can be executed.
        /// </summary>
        /// <returns>an <see cref="EvaluationResult"/> </returns>
        EvaluationResult Evaluate();
    }
}