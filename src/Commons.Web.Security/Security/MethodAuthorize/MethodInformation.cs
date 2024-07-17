using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Web.Security.MethodAuthorize;

/// <summary>
/// Gets and holds information about a method.
/// </summary>
public class MethodInformation
{
    private readonly string _methodName;
    private readonly List<MethodParameter> _methodParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodInformation"/> class.
    /// </summary>
    /// <param name="methodExpression">The method expression.</param>
    /// <param name="parameters">The parameters.</param>
    public MethodInformation(string methodExpression, IDictionary<string, object?> parameters)
    {
        _methodParameters = new List<MethodParameter>();
        _methodName = ParseMethodName(methodExpression);
        _methodParameters = ParseMethodParameters(methodExpression, parameters);
    }

    /// <summary>
    /// Parses the method parameters.
    /// </summary>
    /// <param name="methodExpression">The method expression.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The list of method parameters.</returns>
    private List<MethodParameter> ParseMethodParameters(string methodExpression, IDictionary<string, object?> parameters)
    {
        string parametersBlock = GetParamtersBlock(methodExpression);
        List<MethodParameter> methodParameters = GetMethodParameters(parametersBlock, parameters);
        return methodParameters;
    }

    /// <summary>
    /// Gets the method parameters.
    /// </summary>
    /// <param name="parameterBlock">The parameter block.</param>
    /// <param name="callingParameters">The calling parameters.</param>
    /// <returns>The list of method parameters.</returns>
    private static List<MethodParameter> GetMethodParameters(string parameterBlock, IDictionary<string, object?> callingParameters)
    {
        List<MethodParameter> methodParameters = new List<MethodParameter>();
        string[] parameters = parameterBlock.Split(',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].Trim().StartsWith("#"))
            {
                // Determine type using the method parameters
                string parameterName = parameters[i].Trim(['#', ' ']);
                string rootParameterName = parameterName.Split('.').First();
                object? actionParameter = callingParameters[rootParameterName];

                // If it is a property of the parameter.
                if (parameterName.Contains('.') && actionParameter != null)
                {
                    string parameterExpression = parameterName.Substring(parameterName.IndexOf('.') + 1);
                    actionParameter = ExpressionEvaluator.GetValue(actionParameter, parameterExpression);
                }
                if (actionParameter == null)
                {
                    string errorMessage =
                            string.Format(
                                    "Can't determine type of action parameter. A value of null for the action parameter {0} is not valid.",
                                    parameterName);
                    throw new MethodAuthorizeException(errorMessage);
                }
                else
                {
                    methodParameters.Add(new MethodParameter(parameterName, actionParameter.GetType(), actionParameter));
                }
            }
            else if (parameters[i].Trim().StartsWith("'"))
            {
                methodParameters.Add(new MethodParameter("Unnamed", typeof(string), parameters[i].Trim().Trim('\'')));
            }
            else
            {
                string errorMessage = string.Format("Can't determine type from actionExpression parameters ({0}).", parameters[i]);
                throw new InvalidOperationException(errorMessage);
            }
        }
        return methodParameters;
    }

    /// <summary>
    /// Gets the parameters block from the method expression.
    /// </summary>
    /// <param name="methodExpression">The method expression.</param>
    /// <returns>The parameters block.</returns>
    private static string GetParamtersBlock(string methodExpression)
    {
        int startOfParameters = methodExpression.IndexOf('(');
        int endOfParameters = methodExpression.LastIndexOf(')');

        string parametersBlock = methodExpression.Substring(startOfParameters + 1, endOfParameters - startOfParameters - 1);
        return parametersBlock;
    }

    /// <summary>
    /// Gets the method name from the method expression.
    /// </summary>
    /// <param name="methodExpression">The method expression.</param>
    /// <returns>The method name.</returns>
    private static string ParseMethodName(string methodExpression)
    {
        // extract the methodname from the method expression
        int index = methodExpression.IndexOf('(');
        if (index == -1)
        {
            throw new InvalidOperationException("The method expression is not valid.");
        }
        return methodExpression.Substring(0, index);
    }

    /// <summary>
    /// Gets the method name.
    /// </summary>
    public string MethodName
    {
        get { return _methodName; }
    }

    /// <summary>
    /// Gets the method parameters.
    /// </summary>
    public List<MethodParameter> MethodParameters
    {
        get { return _methodParameters; }
    }
}
