using System;
using System.Diagnostics;
using Xunit;

namespace NSimpleTester.Tests
{
    public class TypeFactoryTests
    {
        [Fact]
        public void CanCreateInstance_returns_true_for_value_types()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreateInstance(typeof(DateTime)));            
        }

        [Fact]
        public void CanCreateInstance_returns_true_for_non_generic_reference_types_with_default_constructor()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreateInstance(typeof(TypeFactory)));
        }

        [Fact]
        public void CanCreateInstance_returns_false_for_non_generic_reference_types_without_default_constructor()
        {
            var sut = new TypeFactory();

            Assert.False(sut.CanCreateInstance(typeof(PropertyTester)));
        }

        [Fact]
        public void CanCreateInstance_returns_true_for_string()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreateInstance(typeof(string)));
        }

        [Fact]
        public void CanCreateInstance_returns_true_for_Type()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreateInstance(typeof(Type)));
        }

        [Fact]
        public void CanCreateInstance_returns_true_for_array()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreateInstance(typeof(PropertyTester[])));
        }

        [Fact]
        public void CanCreateInstance_returns_false_for_generic_type()
        {
            var sut = new TypeFactory();

            Assert.False(sut.CanCreateInstance(createGenericType<PropertyTester>()));
        }

        [Fact]
        public void CreateRandomValue_throws_exception_when_type_null()
        {
            var sut = new TypeFactory();

            Assert.Throws<ArgumentNullException>(() => sut.CreateRandomValue(type: null));
        }

        [Fact]
        public void CreateRandomValue_returns_array_when_type_is_Array()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(DateTime[]));

            Assert.IsAssignableFrom<Array>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_enum_when_type_is_enum()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(TestEnum.One.GetType());

            Assert.IsAssignableFrom<Enum>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_guid_when_type_is_guid()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(Guid.NewGuid().GetType());

            Assert.IsAssignableFrom<Guid>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_type_when_type_is_type()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(Type));

            Assert.IsAssignableFrom<Type>(newValue);
        }
        //!!! need to test all variations of generateNewType

        [Fact]
        public void CreateRandomValue_returns_timeSpan_when_type_is_timeSpan()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(TimeSpan));

            Assert.IsAssignableFrom<TimeSpan>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_bool_when_type_is_bool()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(bool));

            Assert.IsAssignableFrom<bool>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_byte_when_type_is_byte()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(byte));

            Assert.IsAssignableFrom<byte>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_char_when_type_is_char()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(char));

            Assert.IsAssignableFrom<char>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_dateTime_when_type_is_dateTime()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(DateTime));

            Assert.IsAssignableFrom<DateTime>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_decimal_when_type_is_decimal()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(decimal));

            Assert.IsAssignableFrom<decimal>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_double_when_type_is_double()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(double));

            Assert.IsAssignableFrom<double>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_int16_when_type_is_int16()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(Int16));

            Assert.IsAssignableFrom<Int16>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_int32_when_type_is_int32()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(Int32));

            Assert.IsAssignableFrom<Int32>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_int64_when_type_is_int64()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(Int64));

            Assert.IsAssignableFrom<Int64>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_sbyte_when_type_is_sbyte()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(sbyte));

            Assert.IsAssignableFrom<sbyte>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_single_when_type_is_single()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(Single));

            Assert.IsAssignableFrom<Single>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_string_when_type_is_string()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(string));

            Assert.IsAssignableFrom<string>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_unint16_when_type_is_unint16()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(UInt16));

            Assert.IsAssignableFrom<UInt16>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_unint32_when_type_is_unint32()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(UInt32));

            Assert.IsAssignableFrom<UInt32>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_unint64_when_type_is_unint64()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(UInt64));

            Assert.IsAssignableFrom<UInt64>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_custom_type_when_type_is_custom_type()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(TypeFactoryTests));

            Assert.IsAssignableFrom<TypeFactoryTests>(newValue);
        }

        private static Type createGenericType<T>()
        {
            return typeof(T);
        }

        private enum TestEnum { One, Two, Three };
    }
}
