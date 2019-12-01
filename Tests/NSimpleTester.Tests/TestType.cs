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
using System;

namespace NSimpleTester.Tests
{
    public class TestType
    {
        public int? NullableInt { get; set; }

        public TestType[] Array { get; set; }

        public TestEnum Enum { get; set; }

        public Guid Guid { get; set; }

        //!!!public Type Type { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public bool Bool { get; set; }

        public byte Byte { get; set; }

        public char Char { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public Int16 Int16 { get; set; }

        public Int32 Int32 { get; set; }

        public Int64 Int64 { get; set; }

        public int ReadOnly { get; } = 13;

        public SByte SByte { get; set; }

        public Single Single { get; set; }

        public string String { get; set; }

        public UInt16 UInt16 { get; set; }

        public UInt32 UInt32 { get; set; }

        public UInt64 UInt64 { get; set; }

        private int _setOnly = -1;
        public int SetOnly { set => _setOnly = value; }
    }
}
