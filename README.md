# Goals
**NSimpleTester** simplifies testing the stuff you don't want to spend time creating unit tests for. For now, it's just testing properties, including raising INotifyPropertyChanged events and updating backing fields properly. Given that many of us use [c# automatic properties](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties), which are virtually impossible to mess up, the biggest benefit for many will be just to get the unit test coverage numbers up with minimal effort. 

# Usage
Using NSimpleTester is, well, uh, simple:

```c#
[Fact]
public void Properties_configured_correctly()
{
  new PropertyTester(new AwesomeClass()).TestProperties();
}
```
The simple unit test above verifies that all of your properties:
- can be set and retrieved, as indicated on the property
- raise PropertyChangedEvent for classes that implement INotifyPropertyChanged

If any issues are found, an InvalidOperationException is raised and will fail your unit test. If you come across a situation where a property is causing an error, you can exclude the property from testing like so:
```c#
public void Properties_configured_correctly()
{
   var propertyTester = new PropertyTester(new AwesomeClass());
   propertyTester.IgnoredProperties.Add("PropertyOfTypeWithNoDefaultConstructor");

   propertyTester.TestProperties();
}

```
There are currently 3 types of properties that can't be tested:
1. Property type doesn't have a default constructor.
2. Property type is a [generic type definition](https://docs.microsoft.com/en-us/dotnet/api/system.type.isgenerictypedefinition?view=netframework-4.8)
3. The `PropertyTester` is unable to create 2 different values for the type that implements INotifyPropertyChanged. (I can't think of a situation that would cause this, but it's technically possible)
