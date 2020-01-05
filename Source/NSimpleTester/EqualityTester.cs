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
using System.Reflection;

// Based on http://codinghelmet.com/articles/testing-equals-and-gethashcode

namespace NSimpleTester
{
    public static class EqualityTester
    {
        public static bool TestEqualObjects<T>(T obj1, T obj2, IList<string> errorList = null)
        {
            throwIfAnyIsNull(errorList, $"{nameof(TestUnequalObjects)}<{typeof(T).Name}>", obj1, obj2);
            var initialErrorCount = errorList?.Count ?? 0;

            testGetHashCodeOnEqualObjects(errorList, obj1, obj2);
            testEquals(errorList, obj1, obj2, expectedEqual: true);
            testEqualsOfT(errorList, obj1, obj2, expectedEqual: true);
            testEqualityOperator(errorList, obj1, obj2, expectedEqual: true);
            testInequalityOperator(errorList, obj1, obj2, expectedUnequal: false);

            return (errorList?.Count ?? 0) <= initialErrorCount;
        }

        public static bool TestUnequalObjects<T>(T obj1, T obj2, IList<string> errorList = null)
        {
            throwIfAnyIsNull(errorList, $"{nameof(TestUnequalObjects)}<{typeof(T).Name}>", obj1, obj2);
            var initialErrorCount = errorList?.Count ?? 0;

            testEqualsReceivingNonNullOfOtherType(errorList, obj1);
            testEquals(errorList, obj1, obj2, expectedEqual: false);
            testEqualsOfT(errorList, obj1, obj2, expectedEqual: false);
            testEqualityOperator(errorList, obj1, obj2, expectedEqual: false);
            testInequalityOperator(errorList, obj1, obj2, expectedUnequal: true);

            return (errorList?.Count ?? 0) <= initialErrorCount;
        }

        public static bool TestAgainstNull<T>(T obj, IList<string> errorList = null)
        {
            throwIfAnyIsNull(errorList, $"{nameof(TestAgainstNull)}<{typeof(T).Name}>", obj);
            var initialErrorCount = errorList?.Count ?? 0;

            testEqualsReceivingNull(errorList, obj);
            testEqualsOfTReceivingNull(errorList, obj);
            testEqualityOperatorReceivingNull(errorList, obj);
            testInequalityOperatorReceivingNull(errorList, obj);

            return (errorList?.Count ?? 0) <= initialErrorCount;
        }

        private static void testGetHashCodeOnEqualObjects<T>(ICollection<string> errorList, T obj1, T obj2)
        {
            if (ErrorHandler.SafeCall(() => obj1.GetHashCode() != obj2.GetHashCode()))
            { ErrorHandler.Handle(errorList, $"{obj1.GetType().Name}.GetHashCode of equal objects returned different values."); }
        }

        private static void testEqualsReceivingNonNullOfOtherType<T>(ICollection<string> errorList, T obj)
        {
            if (ErrorHandler.SafeCall(() => obj.Equals(new object())))
            { ErrorHandler.Handle(errorList, $"{obj.GetType().Name}.Equals returned true when comparing with object of a different type."); }
        }

        private static void testEqualsReceivingNull<T>(ICollection<string> errorList, T obj)
        {
            if (typeof(T).IsClass)
            { testEquals(errorList, obj, obj2: default, expectedEqual: false); }
        }

        private static void testEqualsOfTReceivingNull<T>(ICollection<string> errorList, T obj)
        {
            if (typeof(T).IsClass)
            { testEqualsOfT(errorList, obj, obj2: default, expectedEqual: false); }
        }

        private static void testEquals<T>(ICollection<string> errorList, T obj1, T obj2, bool expectedEqual)
        {
            if (ErrorHandler.SafeCall(() => obj1.Equals(obj2)) != expectedEqual)
            { ErrorHandler.Handle(errorList, $"{obj1.GetType().Name}.Equals returns {!expectedEqual} " + $"on {(expectedEqual ? "" : "non-")}equal objects."); }
        }

        private static void testEqualsOfT<T>(ICollection<string> errorList, T obj1, T obj2, bool expectedEqual)
        {
            if (obj1 is IEquatable<T> equatable)
            { testEqualsOfTOnIEquatable(equatable, obj2, expectedEqual, errorList); }
        }

        private static void testEqualsOfTOnIEquatable<T>(IEquatable<T> obj1, T obj2, bool expectedEqual, ICollection<string> errorList)
        {
            if (ErrorHandler.SafeCall(() => obj1.Equals(obj2)) != expectedEqual)
            { ErrorHandler.Handle(errorList, $"Strongly typed {obj1.GetType().Name}.Equals returns {!expectedEqual} on {(expectedEqual ? "" : "non-")}equal " + "objects."); }
        }

        private static void testEqualityOperatorReceivingNull<T>(ICollection<string> errorList, T obj)
        {
            if (typeof(T).IsClass)
            { testEqualityOperator(errorList, obj, obj2: default, expectedEqual: false); }
        }

        private static void testEqualityOperator<T>(ICollection<string> errorList, T obj1, T obj2, bool expectedEqual)
        {
            var type = obj1.GetType();
            var equalityOperator = type.GetEqualityOperator();

            if (equalityOperator == null)
            {
                if (type.IsEqualsOverridden())
                { ErrorHandler.Handle(errorList, $"Type '{type.Name}' does not override equality operator."); }
            }
            else
            { testEqualityOperator(obj1, obj2, expectedEqual, equalityOperator, errorList); }
        }

        private static void testEqualityOperator<T>(T obj1, T obj2, bool expectedEqual, MethodBase equalityOperator, ICollection<string> errorList)
        {
            if (ErrorHandler.SafeCall(() => expectedEqual != (bool) equalityOperator.Invoke(obj: null, new object[] { obj1, obj2 })))
            { ErrorHandler.Handle(errorList, $"{obj1.GetType().Name} equality operator returned {!expectedEqual} on {(expectedEqual ? "" : "non-")}equal objects."); }
        }

        private static void testInequalityOperatorReceivingNull<T>(ICollection<string> errorList, T obj)
        {
            if (typeof(T).IsClass)
            { testInequalityOperator(errorList, obj, obj2: default, expectedUnequal: true); }
        }

        private static void testInequalityOperator<T>(ICollection<string> errorList
          , T obj1
          , T obj2
          , bool expectedUnequal)
        {
            var type = obj1.GetType();
            var inequalityOperator = type.GetInequalityOperator();

            if (inequalityOperator == null)
            {
                if (type.IsEqualsOverridden())
                { ErrorHandler.Handle(errorList, $"Type '{obj1.GetType().Name}' does not override inequality operator."); }
            }
            else
            { testInequalityOperator(obj1, obj2, expectedUnequal, inequalityOperator, errorList); }
        }

        private static void testInequalityOperator<T>(T obj1, T obj2, bool expectedUnequal, MethodBase inequalityOperator, ICollection<string> errorList)
        {
            if (!ErrorHandler.SafeCall(() => expectedUnequal == (bool) inequalityOperator.Invoke(obj: null, new object[] { obj1, obj2 })))
            { ErrorHandler.Handle(errorList, $"{obj1.GetType().Name} inequality operator returned {!expectedUnequal} when comparing {(expectedUnequal ? "non-" : "")}equal objects."); }
        }

        private static void throwIfAnyIsNull(ICollection<string> errorList, string context, params object[] objects)
        {
            if (objects.Any(o => o is null))
            { ErrorHandler.Handle(errorList, new ArgumentNullException($"{context} has a null parameter.")); }
        }
    }
}
