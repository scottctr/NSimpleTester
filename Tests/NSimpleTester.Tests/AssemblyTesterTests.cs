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
    public class AssemblyTesterTests
    {
        [Fact]
        public void Constructor_creates_instance()
        {
            Assert.IsAssignableFrom<AssemblyTester>(new AssemblyTester(GetType().Assembly));
        }

        [Fact]
        public void ExcludeClass_prevents_executing_any_tests_on_a_class()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.ConstructorErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());
            
            sut.ExcludeClass("ConstructorParameterNotMapped");

            sut.TestAssembly();
        }

        [Fact]
        public void ExcludeConstructor_prevents_any_constructor_tests()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.ConstructorErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());

            sut.ExcludeConstructor("ConstructorParameterNotMapped", new MethodSignature(new Type[] {}));

            sut.TestAssembly();
        }

        [Fact]
        public void ExcludeConstructorTests_prevents_any_constructor_tests()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.ConstructorErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());
            
            sut.ExcludeConstructorTests();

            sut.TestAssembly();
        }

        [Fact]
        public void ExcludeEqualityTests_excludes_class_from_equity_tests()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.EqualityErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());

            sut.ExcludeEqualityTests("InvalidEquals");

            sut.TestAssembly();
        }

        [Fact]
        public void ExcludeEqualityTests_prevents_any_equality_tests()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.EqualityErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());
            
            sut.ExcludeEqualityTests();

            sut.TestAssembly();
        }

        [Fact]
        public void ExcludeProperty_prevents_testing_a_property()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.PropertyErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());

            sut.ExcludeProperty("TestTypeWBadINotifyPropertyChanged", "PropertyTwo");

            sut.TestAssembly();
        }

        [Fact]
        public void ExcludePropertyTests_prevents_any_property_tests()
        {
            var sut = new AssemblyTester(Assembly.Load("NSimpleTester.Tests.PropertyErrors"));
            // verity the assembly has issues first so we know the test is valid.
            Assert.Throws<InvalidOperationException>(() => sut.TestAssembly());
            
            sut.ExcludePropertyTests();

            sut.TestAssembly();
        }

        [Fact]
        public void TestClass_tests_all_classes()
        {
            new AssemblyTester(GetType().Assembly)
               .ExcludeProperty("TestTypeWBadINotifyPropertyChanged", "PropertyTwo")
               .ExcludeProperty("TestTypeWInvalidPropertySetter", "BadSetterProperty")
               .ExcludeMappedProperties("TestTypeWInvalidPropertySetter")
               .TestAssembly();
        }
    }
}
