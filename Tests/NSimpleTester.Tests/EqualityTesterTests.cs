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
    public class EqualityTesterTests
    {
        [Fact]
        public void TestEqualObjects_throws_exception_if_obj1_null()
        {
            Assert.Throws<ArgumentNullException>(() => EqualityTester.TestEqualObjects(obj1: null, new TestType()));
        }

        [Fact]
        public void TestEqualObjects_throws_exception_if_obj2_null()
        {
            Assert.Throws<ArgumentNullException>(() => EqualityTester.TestEqualObjects(obj1: new TestType(), obj2: null));
        }

        [Fact]
        public void TestEqualObjects_does_not_throw_exception_when_objects_equal()
        {
            var obj1 = new MethodSignature(typeof(int), typeof(string));
            var obj2 = new MethodSignature(typeof(int), typeof(string));

            EqualityTester.TestEqualObjects(obj1, obj2);
        }

        [Fact]
        public void TestEqualObjects_throws_exception_when_objects_not_equal()
        {
            var obj1 = new MethodSignature(typeof(int), typeof(string));
            var obj2 = new MethodSignature(typeof(DateTime), typeof(decimal));

            Assert.Throws<InvalidOperationException>(() => EqualityTester.TestEqualObjects(obj1, obj2));
        }

        [Fact]
        public void TestUnequalObjects_throws_exception_if_obj1_null()
        {
            Assert.Throws<ArgumentNullException>(() => EqualityTester.TestUnequalObjects(obj1: null, new TestType()));
        }

        [Fact]
        public void TestUnequalObjects_throws_exception_if_obj2_null()
        {
            Assert.Throws<ArgumentNullException>(() => EqualityTester.TestUnequalObjects(obj1: new TestType(), obj2: null));
        }

        [Fact]
        public void TestUnequalObjects_does_not_throw_exception_when_objects_equal()
        {
            var obj1 = new MethodSignature(typeof(int), typeof(string));
            var obj2 = new MethodSignature(typeof(int), typeof(string));

            Assert.Throws<InvalidOperationException>(() => EqualityTester.TestUnequalObjects(obj1, obj2));
        }

        [Fact]
        public void TestUnequalObjects_throws_exception_when_objects_not_equal()
        {
            var obj1 = new MethodSignature(typeof(int), typeof(string));
            var obj2 = new MethodSignature(typeof(DateTime), typeof(decimal));

            EqualityTester.TestUnequalObjects(obj1, obj2);
        }

        [Fact]
        public void TestAgainstNull_throws_exception_if_obj_null()
        {
            Assert.Throws<ArgumentNullException>(() => EqualityTester.TestAgainstNull<MethodSignature>(obj: null));
        }
    }
}
