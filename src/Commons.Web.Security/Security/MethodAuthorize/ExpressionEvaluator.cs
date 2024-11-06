using System;
using System.Linq.Expressions;

namespace Queo.Commons.Web.Security.MethodAuthorize
{
    /// <summary>
    /// Provides methods for evaluating expressions.
    /// </summary>
    internal static class ExpressionEvaluator
    {
        /// <summary>
        /// Gets the value of the specified expression from the given object.
        /// </summary>
        /// <param name="item">The object from which to get the value.</param>
        /// <param name="expression">The expression specifying the value to get.</param>
        /// <returns>The value of the specified expression.</returns>
        public static object? GetValue(object item, string expression)
        {
            Type itemType = item.GetType();
            var parameter = Expression.Parameter(itemType, "item");
            Expression property = parameter;
            foreach (var propName in expression.Split('.'))
            {
                property = Expression.PropertyOrField(property, propName);
            }
            var lambda = Expression.Lambda(property, parameter);
            Type type = lambda.GetType();
            dynamic compilable = Convert.ChangeType(lambda, type);
            dynamic func = compilable.Compile();
            dynamic typedItem = Convert.ChangeType(item, itemType);
            return func(typedItem);
        }
    }
}