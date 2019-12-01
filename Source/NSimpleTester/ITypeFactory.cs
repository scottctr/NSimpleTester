using System;

namespace NSimpleTester
{
    public interface ITypeFactory
    {
        bool CanCreateInstance(Type type);

        object CreateRandomValue(Type type);
    }
}