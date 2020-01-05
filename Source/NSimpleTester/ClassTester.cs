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
using System.ComponentModel;
using System.Reflection;

namespace NSimpleTester
{
    public sealed class ClassTester
    {
        private readonly bool _iNotifyPropertyChanged;
        private readonly object _subject;
        private readonly Type _subjectType;
        private readonly ITypeFactory _typeFactory;
        private string _lastPropertyChanged;

        public ClassTester(object subject)
            : this(subject, new TypeFactory())
        { }

        public ClassTester(object subject, ITypeFactory typeFactory)
        {
            _subject = subject ?? throw new ArgumentNullException(nameof(subject));
            _typeFactory = typeFactory ?? throw new ArgumentNullException(nameof(typeFactory));

            _subjectType = subject.GetType();
            IgnoredProperties = new List<string>();

            if (_subjectType.GetInterface(typeof(INotifyPropertyChanged).FullName) != null)
            {
                _iNotifyPropertyChanged = true;
                var propertyChangeSubject = (INotifyPropertyChanged)subject;
                propertyChangeSubject.PropertyChanged += propertyChanged;
            }
        }

        public List<MethodSignature> IgnoredConstructors { get; } = new List<MethodSignature>();

        /// <summary>
        /// Gets a list of Property names to be ignored when this class is tested.
        /// </summary>
        public List<string> IgnoredProperties { get; }

        /// <summary>
        /// When trying to create random values, how many attempts should the algorithm
        /// have at creating different values before erroring.
        /// </summary>
        public int MaxLoopsPerProperty { get; } = 1000;

        public bool TestConstructors(bool testMappedProperties = true, IList<string> errorList = null)
        {
            var initialErrorCount = errorList?.Count ?? 0;

            var constructors = _subjectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var signature = new MethodSignature(parameters);

                if (IgnoredConstructors.Contains(signature))
                { break; }

                testConstructor(constructor, parameters, signature, testMappedProperties, errorList);
            }

            return (errorList?.Count ?? 0) <= initialErrorCount;
        }

        public bool TestEquality(IList<string> errorList = null)
        {
            var initialErrorCount = errorList?.Count ?? 0;

            if (!_typeFactory.CanCreateInstance(_subjectType))
            {
                ErrorHandler.Handle(errorList, $"Unable to create instance of {_subjectType} for {nameof(TestEquality)}.");
                return false;
            }

            _typeFactory.CreateDualInstances(_subjectType, out var instance1, out var instance1Clone);

            EqualityTester.TestEqualObjects(instance1, instance1Clone);
            EqualityTester.TestUnequalObjects(instance1, _subject);
            EqualityTester.TestAgainstNull(_subject);

            return (errorList?.Count ?? 0) <= initialErrorCount;
        }

        public bool TestProperties(IList<string> errorList = null)
        {
            var initialErrorCount = errorList?.Count ?? 0;

            var properties = _subjectType.GetProperties();
            foreach (var property in properties)
            {
                // we can only SET if the property is writable and we can new up the type
                var canSet = property.CanWrite;
                var canGet = property.CanRead;

                if (ignoreProperty(property) || propertyIsIndexed(property))
                {
                    // skip this property - we can't test indexers or properties
                    // we've been asked to ignore
                    continue;
                }

                object valueIn2 = null;

                // we can only set properties 
                if (canSet)
                {
                    if (!_typeFactory.CanCreateInstance(property.PropertyType))
                    { ErrorHandler.Handle(errorList, $"Cannot generate type '{property.PropertyType}' to set on property '{property.Name}'. Consider ignoring this property on the type '{_subjectType.Name}'"); }

                    // We need two 'in' values to ensure the property actually changes.
                    // because the values are random - we need to loop to make sure we
                    // get different ones (i.e. bool);

                    object valueIn1;
                    try
                    {
                        valueIn1 = valueIn2 = _typeFactory.CreateRandomValue(property.PropertyType);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(errorList, $"Error creating value for {_subjectType.Name}.{property.Name}: {ex.Message}.");
                        continue;
                    }

                    if (_iNotifyPropertyChanged)
                    {
                        // safety net 
                        var counter = 0;
                        while (valueIn2.Equals(valueIn1))
                        {
                            if (counter++ > MaxLoopsPerProperty)
                            {
                                ErrorHandler.Handle(errorList, $"Could not generate two different values for the type '{property.PropertyType}'. Consider ignoring the '{property.Name}' property on the type '{_subjectType.Name}' or increasing the MaxLoopsPerProperty value above {MaxLoopsPerProperty}"); }
                            valueIn2 = _typeFactory.CreateRandomValue(property.PropertyType);
                        }
                    }

                    property.SetValue(_subject, valueIn1, index: null);
                    if (_iNotifyPropertyChanged)
                    { property.SetValue(_subject, valueIn2, index: null); }

                    // This currently assumes single threaded execution - do we need to consider threads here?
                    if (_iNotifyPropertyChanged)
                    {
                        if (_lastPropertyChanged != property.Name)
                        {
                            ErrorHandler.Handle(errorList, $"The property '{property.Name}' on the type '{_subjectType.Name}' did not raise a PropertyChangedEvent"); }
                        _lastPropertyChanged = null;
                    }
                }

                if (!canGet)
                { continue; }

                var valueOut = property.GetValue(_subject, index: null);

                // if we can also write - we should test the value
                // we wrote to the variable.
                if (!canSet)
                { continue; }

                if (!Equals(valueIn2, valueOut))
                { ErrorHandler.Handle(errorList, $"The get value of the '{property.Name}' property on the type '{_subjectType.Name}' did not equal the set value (in: '{valueIn2}', out: '{valueOut}')"); }
            }

            return (errorList?.Count ?? 0) <= initialErrorCount;
        }

        private bool ignoreProperty(MemberInfo property)
        {
            return IgnoredProperties.Contains(property.Name);
        }

        private void propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _lastPropertyChanged = e.PropertyName;
        }

        private bool propertyIsIndexed(PropertyInfo property)
        {
            return property.GetIndexParameters().Length > 0;
        }

        private void testConstructor(ConstructorInfo constructor
          , ParameterInfo[] parameters
          , MethodSignature signature
          , bool testMappedProperties
          , ICollection<string> errorList)
        {
            var paramValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                if (!_typeFactory.CanCreateInstance(parameters[i].ParameterType))
                { ErrorHandler.Handle(errorList, $"Cannot create an instance of the type '{_subjectType.Name}.{parameters[i].ParameterType}' for the parameter '{parameters[i].Name}' in the .ctor{signature} for type {_subjectType}"); }

                paramValues[i] = _typeFactory.CreateRandomValue(parameters[i].ParameterType);
            }

            var result = constructor.Invoke(paramValues);

            if (testMappedProperties)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var paramValue = paramValues[i];
                    testParam(parameter, result, paramValue, errorList);
                }
            }
        }

        private void testParam(ParameterInfo parameter, object result, object paramValue, ICollection<string> errorList)
        {
            var mappedProperty = _subjectType.GetProperty(parameter.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (mappedProperty == null || !mappedProperty.CanRead)
            { return; }

            var valueOut = mappedProperty.GetValue(result, index: null);
            if (!Equals(paramValue, valueOut))
            {
                ErrorHandler.Handle(errorList, $"The value of the '{_subjectType.Name}.{mappedProperty.Name}' property did not equal the value set with the '{parameter.Name}' constructor parameter (in: '{valueOut}', out: '{paramValue}')"); }
        }
    }
}
