using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Commons.Web.Security.MethodAuthorize;
using NUnit.Framework;

namespace Commons.Web.Security.Tests.MethodAuthorize
{

    [TestFixture]
    public class ExpressionEvaluatorTests
    {
        [Test]
        public void GetValue_WhenCalled_ShouldReturnPropertyValue()
        {
            // Arrange
            var item = new TestClass { Property1 = "Value1", Property2 = 2 };
            var expression = "Property1";
            var expected = "Value1";

            // Act
            var result = ExpressionEvaluator.GetValue(item, expression);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetValue_WhenCalledWithNestedProperty_ShouldReturnPropertyValue()
        {
            // Arrange
            var item = new TestClass { Property3 = new TestClass { Property1 = "Value2" } };
            var expression = "Property3.Property1";
            var expected = "Value2";

            // Act
            var result = ExpressionEvaluator.GetValue(item, expression);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    public class TestClass
    {
        public string? Property1 { get; set; }
        public int Property2 { get; set; }

        public TestClass? Property3 { get; set; }
    }
}

