namespace FluentExtractor.Results
{
    public class BooleanDescriptorValue : IDescriptorValue
    {
        public bool Value
        {
            get;
            internal set;
        }

        public BooleanDescriptorValue(bool value) => Value = value;

        public override bool Equals(object obj)
            => obj is BooleanDescriptorValue booleanDescriptorValue
            && booleanDescriptorValue.Value == Value;
    }
}
