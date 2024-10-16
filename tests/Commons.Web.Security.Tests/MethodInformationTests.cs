using Commons.Web.Security.MethodAuthorize;
using NUnit.Framework;

namespace Commons.Web.Security.Tests;
[TestFixture]
public class MethodInformationTests
{
    [Test]
    public void ParseMethodName_WhenCalled_ShouldReturnMethodName()
    {
        // Arrange
        var methodExpression = "MethodName(#param1, #param2)";
        var parameters = new Dictionary<string, object?>
        {
            { "param1", "Wert dazu"},
            { "param2", 42 }
        };
        var methodInformation = new MethodInformation(methodExpression, parameters);

        // Act
        var result = methodInformation.MethodName;

        // Assert
        Assert.That(result, Is.EqualTo("MethodName"));
    }
    [Test]
    public void ParseMethodParameters_WhenCalled_ShouldReturnMethodParameters()
    {
        // Arrange
        var methodExpression = "MethodName(#param1, #param2)";
        var parameters = new Dictionary<string, object?>
        {
            { "param1", "Wert dazu"},
            { "param2", 42 }
        };
        var methodInformation = new MethodInformation(methodExpression, parameters);

        // Act
        var result = methodInformation.MethodParameters;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].ParameterType, Is.EqualTo(typeof(string)));
            Assert.That(result[0].ParameterName, Is.EqualTo("param1"));
            Assert.That(result[0].ParameterValue, Is.EqualTo("Wert dazu"));
            Assert.That(result[1].ParameterType, Is.EqualTo(typeof(int)));
            Assert.That(result[1].ParameterName, Is.EqualTo("param2"));
            Assert.That(result[1].ParameterValue, Is.EqualTo(42));
        });
    }

    [Test]
    public void ParseMethodName_WhenCalledWithString_ShouldReturnMethodName()
    {
        // Arrange
        var methodExpression = "MethodName('paramValue')";
        var parameters = new Dictionary<string, object?>
        { };
        var methodInformation = new MethodInformation(methodExpression, parameters);

        // Act
        var result = methodInformation.MethodName;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("MethodName"));
            Assert.That(methodInformation.MethodParameters[0].ParameterValue, Is.EqualTo("paramValue"));
        });
    }
}
