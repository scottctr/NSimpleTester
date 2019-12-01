#region Copyright (c) 2019 Scott L. Carter
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using Moq;
using System;
using Xunit;

namespace NSimpleTester.Tests
{
    public class PropertyTesterTests
    {
        [Fact]
        public void Constructor_creates_instance()
        {
            Assert.IsType<PropertyTester>(new PropertyTester(new TestType()));
        }

        [Fact]
        public void Constructor_throws_exception_if_subject_null()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyTester(null));
        }

        [Fact]
        public void Constructor_throws_exception_if_typeFactory_null()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyTester(new TestType(), null));
        }

        [Fact]
        public void TestProperties_tests_properties()
        {
            var sut = new PropertyTester(new TestType());

            sut.TestProperties();
        }

        [Fact]
        public void TestProperties_throws_error_when_cant_crete_property_type()
        {
            Assert.Throws<InvalidOperationException>(() => new PropertyTester(new TestTypeWBadProperty())
                .TestProperties());
        }

        [Fact]
        public void TestProperties_throws_error_when_cant_generate_2_different_property_values()
        {
            var mockTypeFactory = new Mock<ITypeFactory>();
            mockTypeFactory.Setup(factory => factory.CanCreateInstance(It.IsAny<Type>())).Returns(value: true);
            mockTypeFactory.Setup(factory => factory.CreateRandomValue(typeof(int))).Returns(value: -1);
            var sut = new PropertyTester(new TestTypeWINotifyPropertyChanged(), mockTypeFactory.Object);

            Assert.Throws<InvalidOperationException>(() => sut.TestProperties());
        }

        [Fact]
        public void TestProperties_throws_error_when_cant_set_property()
        {
            Assert.Throws<InvalidOperationException>(() => new PropertyTester(new TestTypeWInvalidPropertySetter())
                .TestProperties());
        }

        [Fact]
        public void IgnoredProperties_prevents_testing_specified_properties()
        {
            var sut = new PropertyTester(new TestTypeWBadProperty());

            sut.IgnoredProperties.Add("CantCreateInstanceProperty");
            sut.TestProperties();
        }

        [Fact]
        public void TestProperties_works_with_INotifyPropertyChanged()
        {
            new PropertyTester(new TestTypeWINotifyPropertyChanged()).TestProperties();
        }

        [Fact]
        public void TestProperties_throws_error_when_INotifyPropertyChanged_not_configured_correctly()
        {
            Assert.Throws<InvalidOperationException>(() => new PropertyTester(new TestTypeWBadINotifyPropertyChanged()).TestProperties());
        }
    }
}
