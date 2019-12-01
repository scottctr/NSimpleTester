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
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace NSimpleTester
{
    public sealed class PropertyTester
    {
        private readonly bool _iNotifyPropertyChanged;
        private readonly object _subject;
        private readonly Type _subjectType;
        private readonly ITypeFactory _typeFactory;
        private string _lastPropertyChanged;

        public PropertyTester(object subject)
            : this(subject, new TypeFactory())
        { }

        public PropertyTester(object subject, ITypeFactory typeFactory)
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

        /// <summary>
        /// Gets a list of Property names to be ignored when this class is tested.
        /// </summary>
        public List<string> IgnoredProperties { get; }

        /// <summary>
        /// When trying to create random values, how many attempts should the algorithm
        /// have at creating different values before erroring.
        /// </summary>
        public int MaxLoopsPerProperty { get; } = 1000;

        public void TestProperties()
        {
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
                    { throw new InvalidOperationException($"Cannot generate type '{property.PropertyType}' to set on property '{property.Name}'. Consider ignoring this property on the type '{_subjectType}'"); }

                    // We need two 'in' values to ensure the property actually changes.
                    // because the values are random - we need to loop to make sure we
                    // get different ones (i.e. bool);
                    var valueIn1 = valueIn2 = _typeFactory.CreateRandomValue(property.PropertyType);

                    if (_iNotifyPropertyChanged)
                    {
                        // safety net 
                        var counter = 0;
                        while (valueIn2.Equals(valueIn1))
                        {
                            if (counter++ > MaxLoopsPerProperty)
                            {
                                throw new InvalidOperationException(
                                    $"Could not generate two different values for the type '{property.PropertyType}'. Consider ignoring the '{property.Name}' property on the type '{_subjectType}' or increasing the MaxLoopsPerProperty value above {MaxLoopsPerProperty}");
                            }
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
                            throw new InvalidOperationException(
                                $"The property '{property.Name}' on the type '{_subjectType}' did not throw a PropertyChangedEvent");
                        }
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
                {
                    throw new InvalidOperationException(
                        $"The get value of the '{property.Name}' property on the type '{_subjectType}' did not equal the set value (in: '{valueIn2}', out: '{valueOut}')");
                }
            }
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
    }
}
