namespace Commons.Web.Security.ActionDescription;

/// <summary>
/// Parses an identifier expression to it's parts and can build
/// a concrete permission from the expression and an identifier.
/// </summary>
internal class IdentifierExpression
{
    private readonly string _expression;

    /// <summary>
    /// Gets the base expression of the identifier.
    /// </summary>
    public string BaseExpression { get; }

    /// <summary>
    /// Gets the parameter name of the identifier.
    /// </summary>
    public string ParameterName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierExpression"/> class.
    /// </summary>
    /// <param name="expression">The identifier expression.</param>
    public IdentifierExpression(string expression)
    {
        _expression = expression;
        BaseExpression = GetBaseExpression();
        ParameterName = GetParameterName();
    }

    /// <summary>
    /// Builds a concrete expression by replacing the parameter name with the provided identifier.
    /// </summary>
    /// <param name="identifier">The identifier to replace the parameter name with.</param>
    /// <returns>The concrete expression.</returns>
    public string BuildConcreteExpression(string identifier)
    {
        string parameterName = GetParameterName();
        return _expression.Replace($"{{{parameterName}}}", identifier);
    }

    private string GetParameterName()
    {
        return _expression.Substring(_expression.IndexOf('{') + 1, _expression.IndexOf('}') - _expression.IndexOf('{') - 1);
    }

    private string GetBaseExpression()
    {
        string businessIdParameterName = _expression.Substring(0, _expression.IndexOf('{')).Trim('_');
        return businessIdParameterName;
    }
}
