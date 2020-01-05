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
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using Moq;
using System;
using Xunit;

namespace NSimpleTester.Tests
{
    public class ClassTesterTests
    {
        [Fact]
        public void Constructor_creates_instance()
        {
            Assert.IsType<ClassTester>(new ClassTester(new TestType()));
        }

        [Fact]
        public void Constructor_throws_exception_if_subject_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ClassTester(subject: null));
        }

        [Fact]
        public void Constructor_throws_exception_if_typeFactory_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ClassTester(new TestType(), null));
        }

        [Fact]
        public void TestConstructors_tests_constructors()
        {
            new ClassTester(new TestType()).TestConstructors(true);
        }

        [Fact]
        public void IgnoredConstructors_skips_testing_constructor()
        {
            var sut = new ClassTester(new TestTypeWBadProperty());
            sut.IgnoredConstructors.Add(new MethodSignature(typeof(ClassTester)));

            sut.TestConstructors(true);
        }

        [Fact]
        public void TestConstructorWithTestMappedProperties_throws_exception_if_property_not_set()
        {
            var sut = new ClassTester(new TestTypeWInvalidPropertySetter());

            Assert.Throws<InvalidOperationException>(() => sut.TestConstructors(testMappedProperties: true));
        }

        [Fact]
        public void TestProperties_tests_properties()
        {
            var sut = new ClassTester(new TestType());

            sut.TestProperties();
        }

        [Fact]
        public void TestProperties_throws_error_when_cant_generate_2_different_property_values()
        {
            var mockTypeFactory = new Mock<ITypeFactory>();
            mockTypeFactory.Setup(factory => factory.CanCreateInstance(It.IsAny<Type>())).Returns(value: true);
            mockTypeFactory.Setup(factory => factory.CreateRandomValue(typeof(int))).Returns(value: -1);
            var sut = new ClassTester(new TestTypeWINotifyPropertyChanged(), mockTypeFactory.Object);

            Assert.Throws<InvalidOperationException>(() => sut.TestProperties());
        }

        [Fact]
        public void TestProperties_throws_error_when_cant_set_property()
        {
            Assert.Throws<InvalidOperationException>(() => new ClassTester(new TestTypeWInvalidPropertySetter())
                .TestProperties());
        }

        [Fact]
        public void IgnoredProperties_prevents_testing_specified_properties()
        {
            var sut = new ClassTester(new TestTypeWBadProperty());

            sut.IgnoredProperties.Add("CantCreateInstanceProperty");
            sut.TestProperties();
        }

        [Fact]
        public void TestProperties_works_with_INotifyPropertyChanged()
        {
            new ClassTester(new TestTypeWINotifyPropertyChanged()).TestProperties();
        }

        [Fact]
        public void TestProperties_throws_error_when_INotifyPropertyChanged_not_configured_correctly()
        {
            Assert.Throws<InvalidOperationException>(() => new ClassTester(new TestTypeWBadINotifyPropertyChanged()).TestProperties());
        }
    }
}
