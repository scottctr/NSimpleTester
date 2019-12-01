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
