namespace NSimpleTester.Tests
{
    public class TestTypeWInvalidPropertySetter
    {
        private string _backingField;
        public string BadSetterProperty
        {
            get { return string.Empty; }
            set { _backingField = value; }
        }
    }
}
