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
using System;
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

            Assert.True(sut.CanCreateInstance(typeof(TestType)));
        }

        [Fact]
        public void CanCreateInstance_returns_true_for_custom_class_without_default_ctor()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreateInstance(typeof(TestTypeNoDefaultCtor)));
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
        public void CreateRandomValue_returns_uint16_when_type_is_uint16()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(UInt16));

            Assert.IsAssignableFrom<UInt16>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_uint32_when_type_is_uint32()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(UInt32));

            Assert.IsAssignableFrom<UInt32>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_uint64_when_type_is_uint64()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(UInt64));

            Assert.IsAssignableFrom<UInt64>(newValue);
        }

        [Fact]
        public void CreateRandomValue_returns_custom_type_when_type_is_custom_type()
        {
            var sut = new TypeFactory();

            var newValue = sut.CreateRandomValue(typeof(TestTypeNoDefaultCtor));

            Assert.IsAssignableFrom<TestTypeNoDefaultCtor>(newValue);
        }

        private static Type createGenericType<T>()
        {
            return typeof(T);
        }

        private enum TestEnum { One, Two, Three };
    }
}
