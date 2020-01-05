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
using System.Reflection;
using Xunit;

namespace NSimpleTester.Tests
{
    public class MethodSignatureTests
    {
        [Fact]
        public void Simple_stuff_works()
        {
            var tester = new ClassTester(new MethodSignature(typeof(int), typeof(string)));
            tester.IgnoredConstructors.Add(new MethodSignature(typeof(ParameterInfo[])));

            tester.TestConstructors();
            tester.TestProperties();
        }

        [Fact]
        public void Equality_override_works()
        {
            var item1 = new MethodSignature(typeof(int), typeof(string));
            var item2 = new MethodSignature(typeof(int), typeof(string));
            var item3 = new MethodSignature(typeof(DateTime), typeof(decimal));

            EqualityTester.TestEqualObjects(item1, item2);
            EqualityTester.TestUnequalObjects(item2, item3);
            EqualityTester.TestAgainstNull(item3);
        }

        [Fact]
        public void ToString_creates_string()
        {
            var sut = new MethodSignature(typeof(TestTypeNoDefaultCtor).GetConstructors()[0].GetParameters());

            var output = sut.ToString();

            Assert.Contains("(System.Int32, System.String)", output);
        }
    }
}
