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
