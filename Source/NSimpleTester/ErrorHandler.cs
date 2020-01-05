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
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NSimpleTester.Tests")]
namespace NSimpleTester
{
    internal static class ErrorHandler
    {
        public static void Handle(ICollection<string> errorList, string errorMessage)
        {
            if (errorList is null)
            { throw new InvalidOperationException(errorMessage); }

            errorList.Add(errorMessage);
        }

        public static void Handle(ICollection<string> errorList, Exception ex)
        {
            if (errorList is null)
            { throw ex; }

            errorList.Add($"{ex.GetType().Name}: {ex.Message}");
        }

        public static bool SafeCall(Func<bool> test)
        {
            try
            { return test.Invoke(); }
            catch
            { return false; }
        }
    }
}