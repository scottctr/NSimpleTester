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
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NSimpleTester.Tests
{
    public class ErrorHandlerTests
    {
        [Fact]
        public void HandleException_adds_error_to_list()
        {
            var exception = new ArgumentNullException("parameter");
            var errorList = new List<string>();

            ErrorHandler.Handle(errorList, exception);

            Assert.NotNull(errorList.Single(e => e.StartsWith("ArgumentNullException: Value cannot be null")));
        }

        [Fact]
        public void HandleException_throws_exception_if_no_list()
        {
            var exception = new ArgumentNullException("parameter");

            Assert.Throws<ArgumentNullException>(() => ErrorHandler.Handle(errorList: null, exception));
        }

        [Fact]
        public void HandleString_adds_error_to_list()
        {
            const string error = "error message";
            var errorList = new List<string>();

            ErrorHandler.Handle(errorList, error);

            Assert.Contains(error, errorList);
            Assert.Single(errorList);
        }

        [Fact]
        public void HandleString_throws_exception_if_no_list()
        {
            Assert.Throws<InvalidOperationException>(() => ErrorHandler.Handle(errorList: null, "error"));
        }

        [Fact]
        public void SafeCall_returns_false_if_func_throws_exception()
        {
            Assert.False(ErrorHandler.SafeCall(() => throw new InvalidOperationException()));
        }

        [Fact]
        public void SafeCall_returns_func_return_value()
        {
            Assert.False(ErrorHandler.SafeCall(() => false));
            Assert.True(ErrorHandler.SafeCall(() => true));
        }
    }
}
