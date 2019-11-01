namespace dapper.fun.dam.test
{
    public struct Strings
        {
            public Strings(string[] value)
            {
                this.Value = value;
            }
            public string[] Value { get; }
            public static implicit operator Strings(string[] value)
            {
                return new Strings(value);
            }
            public static implicit operator string[](Strings strings)
            {
                return strings.Value;
            }
        }
}
