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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace NSimpleTester
{
    public sealed class TypeFactory: ITypeFactory
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Indicates whether the CreateRandomValue method is able to create an instance of
        /// the specified type.
        /// </summary>
        /// <param name="type">The type to test</param>
        /// <returns>true or false</returns>
        public bool CanCreateInstance(Type type)
        {
            return (hasDefaultConstructor(type) 
                    || type == typeof(string) 
                    || type == typeof(Type) 
                    || type.IsArray) 
                && !type.IsGenericTypeDefinition;
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <returns>A random value or the default instance for unknown types</returns>
        public object CreateRandomValue(Type type)
        {
            if (type is null)
            { throw new ArgumentNullException(nameof(type)); }

            while (true)
            {
                // First, is the type a nullable type? if so return a value based on the
                // generic argument.
                if (isNullableType(type))
                {
                    var subType = type.GetGenericArguments()[0];
                    type = subType;

                    continue;
                }

                if (type.IsArray)
                {
                    return Array.CreateInstance(type.GetElementType() ?? throw new InvalidOperationException()
                      , _random.Next(maxValue: 50));
                }

                if (type.IsEnum)
                { 
                    var values = Enum.GetValues(type);

                    return values.GetValue(_random.Next(values.Length));
                }

                if (type == typeof(Guid))
                { return Guid.NewGuid(); }

                if (type == typeof(Type))
                { return generateNewType(); }

                if (type == typeof(TimeSpan))
                { return new TimeSpan(_random.Next(int.MaxValue)); }

                var typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        return _random.Next(maxValue: 2) == 1;
                    case TypeCode.Byte:
                        return Convert.ToByte(_random.Next(byte.MinValue, byte.MaxValue));
                    case TypeCode.Char:
                        return Convert.ToChar(_random.Next(char.MinValue, char.MaxValue));
                    case TypeCode.DateTime:
                        return new DateTime(_random.Next(int.MaxValue));
                    case TypeCode.Decimal:
                        return Convert.ToDecimal(_random.Next(int.MaxValue));
                    case TypeCode.Double:
                        return _random.NextDouble();
                    case TypeCode.Int16:
                        return Convert.ToInt16(_random.Next(short.MinValue, short.MaxValue));
                    case TypeCode.Int32:
                        return _random.Next(int.MinValue, int.MaxValue);
                    case TypeCode.Int64:
                        return Convert.ToInt64(_random.Next(int.MinValue, int.MaxValue));
                    case TypeCode.SByte:
                        return Convert.ToSByte(_random.Next(sbyte.MinValue, sbyte.MaxValue));
                    case TypeCode.Single:
                        return Convert.ToSingle(_random.Next(sbyte.MinValue, sbyte.MaxValue));
                    case TypeCode.String:
                        return Guid.NewGuid().ToString();
                    case TypeCode.UInt16:
                        return Convert.ToUInt16(_random.Next(minValue: 0, ushort.MaxValue));
                    case TypeCode.UInt32:
                        return Convert.ToUInt32(_random.Next(minValue: 0, int.MaxValue));
                    case TypeCode.UInt64:
                        return Convert.ToUInt64(_random.Next(minValue: 0, int.MaxValue));
                    default:
                        return Activator.CreateInstance(type);
                }
            }
        }

        /// <summary>
        /// Compiles an entirely new Class (with a random name) and returns its System.Type representation
        /// </summary>
        /// <returns>System.Type for a generated, random class</returns>
        private static Type generateNewType()
        {
            var className = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var codeToCompile = @"using System;
                                     namespace NSimpleTester
                                     { 
                                        public class " + className + @" 
                                        { } 
                                     }";

            var syntaxTree = CSharpSyntaxTree.ParseText(codeToCompile);
            var assemblyName = Guid.NewGuid().ToString();

            var refPaths = new[] { typeof(object).GetTypeInfo().Assembly.Location }; 
            var references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var memoryStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(memoryStream);

                if (!emitResult.Success)
                {
                    var failures = emitResult.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error).ToArray();

                    throw new InvalidOperationException(
                        $"There were {failures.Count()} error(s) compiling the new type (first error shown only):{Environment.NewLine}{failures[0].GetMessage()}");
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var assembly = AssemblyLoadContext.Default.LoadFromStream(memoryStream);

                return assembly.GetType("NSimpleTester." + className);
            }
        }

        /// <summary>
        /// Indicates whether a type has a default public constructor, e.g. can be 'new-ed' up 
        /// unlike System.String which cannot
        /// </summary>
        /// <param name="type">The type to test</param>
        /// <returns>true or false</returns>
        private static bool hasDefaultConstructor(Type type)
        {
            // Is this a struct? If so - can be new-ed up.
            if (type.IsSubclassOf(typeof(ValueType)))
            { return true; }

            // otherwise test for an actual default constructor
            return type.GetConstructor(new Type[] { }) != null;
        }

        /// <summary>
        /// Indicates whether the type is a nullable type (e.g. int? or Nullable&lt;int>)
        /// </summary>
        /// <param name="type">The type to test</param>
        /// <returns></returns>
        private static bool isNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
