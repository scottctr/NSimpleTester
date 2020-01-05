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
    public class MethodSignature : IEquatable<MethodSignature>
    {
        /// <summary>
        /// List of parameter types (in order) that represent this method signature. 
        /// Details such as out, ref, params and parameter names are ignored.
        /// </summary>
        public Type[] Types { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="types"></param>
        public MethodSignature(params Type[] types)
        {
            Types = types;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameters">Info about parameters (in order) for this method signature.</param>
        public MethodSignature(params ParameterInfo[] parameters)
        {
            Types = new Type[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            { Types[i] = parameters[i].ParameterType; }
        }

        /// <summary>
        /// Tests for equality by comparing this type's internal Types array against another
        /// MethodSignature's internal Types array. Any other type returns false.
        /// </summary>
        /// <param name="obj">The MethodSignature for testing</param>
        /// <returns>true if equal; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as MethodSignature);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="other">The other MethodSignature to compare this instance to.</param>
        /// <returns>true if equal; otherwise false.</returns>
        public bool Equals(MethodSignature other)
        {
            if (other == null) { return false; }

            if (ReferenceEquals(other, this))
            { return true; }

            if (other.Types.Length != Types.Length)
            { return false; }

            return !other.Types.Where((t, i) => t != Types[i]).Any();
        }

        /// <summary>
        /// Generates a hashcode for this instance.
        /// </summary>
        /// <returns>Int32</returns>
        public override int GetHashCode()
        {
            if (Types is null)
            { return 0.GetHashCode(); }

            return Types.Any() 
                ? Types.Aggregate(seed: 0, (current, type) => current ^ type.GetHashCode()) 
                : Types.GetHashCode();
        }

        public static bool operator ==(MethodSignature obj1, MethodSignature obj2)
        {
            if (obj1 is null)
            { return obj2 is null; }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(MethodSignature obj1, MethodSignature obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Generates a string that would identify an overload of a method signature, 
        /// e.g. (int, int, string, object)
        /// </summary>
        /// <returns>list of types in parentheses</returns>
        public override string ToString()
        {
            var methodSignature = "(";

            for (var i = 0; i < Types.Length; i++)
            {
                methodSignature += Types[i].ToString();

                if (i != Types.Length - 1)
                { methodSignature += ", "; }
            }

            return methodSignature + ")";
        }
    }
}
