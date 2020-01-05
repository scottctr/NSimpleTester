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
using System.Linq;
using System.Reflection;

namespace NSimpleTester
{
    public static class TypeExtensions
    {
        public static MethodInfo GetEqualityOperator(this Type type)
        {
            return getOperator(type, "op_Equality");
        }

        public static MethodInfo GetInequalityOperator(this Type type)
        {
            return getOperator(type, "op_Inequality");
        }

        public static bool IsEqualsOverridden(this Type type)
        {
            if (type.IsEnum) { return false; }

            var equalsMethod = type.GetMethods().Single(m => m.Name == "Equals");

            return isOverride(equalsMethod);
        }

        private static MethodInfo getOperator(IReflect type, string methodName)
        {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
            var equalityOperator = type.GetMethod(methodName, bindingFlags);
            return equalityOperator;
        }

        private static bool isOverride(MethodInfo methodInfo)
        {
            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }
    }
}
