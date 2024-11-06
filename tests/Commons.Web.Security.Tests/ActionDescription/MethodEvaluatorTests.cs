using System.Reflection;

using Queo.Commons.Web.Security.ActionDescription;
using Queo.Commons.Web.Security.MethodAuthorize;
using NUnit.Framework;

namespace Queo.Commons.Web.Security.Tests.ActionDescription;
public class MethodEvaluatorTests
{
    private MethodInfo _matchingMethod;
    private MethodInformation _methodInformation;
    private object _methodSecurityRoot;

    [SetUp]
    public void Setup()
    {
        _matchingMethod = typeof(MyClass).GetMethod("MyMethod")!;
        _methodInformation = new MethodInformation("MyMethod()", new Dictionary<string, object?>());
        _methodSecurityRoot = new MyClass();
    }

    [Test]
    public void Evaluate_ShouldReturnEvaluationResult()
    {
        // Arrange
        var evaluator = new MethodEvaluator(_matchingMethod, _methodInformation, _methodSecurityRoot);

        // Act
        var result = evaluator.Evaluate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("MyMethod"));
            Assert.That(result.CanExecute, Is.True);
        });
    }

    [Test]
    public void GetActionName_WhenActionNameAttributeExists_ShouldReturnActionName()
    {
        // Arrange
        var method = typeof(MyClass).GetMethod("MyMethodWithAttribute");
        var methodInformation = new MethodInformation("MyMethodWithAttribute()", new Dictionary<string, object?>());
        var evaluator = new MethodEvaluator(method!, methodInformation, _methodSecurityRoot);

        // Act
        var actionName = evaluator.ActionName;

        // Assert
        Assert.That(actionName, Is.EqualTo("CustomAction"));
    }

    [Test]
    public void GetActionName_WhenActionNameAttributeDoesNotExist_ShouldReturnMethodName()
    {
        // Arrange
        var method = typeof(MyClass).GetMethod("MyMethod");
        var methodInformation = new MethodInformation("MyMethod()", new Dictionary<string, object?>());
        var evaluator = new MethodEvaluator(method!, methodInformation, _methodSecurityRoot);

        // Act
        var actionName = evaluator.ActionName;

        // Assert
        Assert.That(actionName, Is.EqualTo("MyMethod"));
    }

    [Test]
    public void Evaluate_ShouldReturnFalse_WhenMethodWithAttributeIsEvaluated()
    {
        // Arrange
        var method = typeof(MyClass).GetMethod("MyMethodWithAttribute");
        var methodInformation = new MethodInformation("MyMethodWithAttribute()", new Dictionary<string, object?>());
        var evaluator = new MethodEvaluator(method!, methodInformation, _methodSecurityRoot);

        // Act
        var result = evaluator.Evaluate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("CustomAction"));
            Assert.That(result.CanExecute, Is.False);
        });
    }
}

public class MyClass
{
    public bool MyMethod()
    {
        // Method implementation
        return true;
    }

    [ActionName("CustomAction")]
    public bool MyMethodWithAttribute()
    {
        // Method implementation
        return false;
    }
}
