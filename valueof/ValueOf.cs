using System.Collections.Generic;
using System.Linq;

// https://github.com/mcintyre321/ValueOf/blob/master/ValueOf/ValueOf.cs
namespace valueof
{
    public static class ValueOf
    {
        public static ValueOf<TValue> From<TValue>(TValue value)
        {
            return new ValueOf<TValue>(value);
        }
        public static ValueOf<TValue> ToValueOf<TValue>(this TValue value)
        {
            return From(value);
        }
        public static bool IsValueOf<TValue>(this object value, ValueOf<TValue> valueof)
        {
            return value is ValueOf<TValue>;
        }
        public static bool IsEmpty<TValue>(this ValueOf<TValue> valueof)
        {
            return valueof.Value?.Equals(default) ?? true;
        }
    }
    public struct ValueOf<TValue>
    {
        public ValueOf(TValue value = default)
        {
            Value = value;
        }
        public TValue Value { get; }

        public override string ToString()
        {
            return Value?.ToString();
        }
        public bool Equals(ValueOf<TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value) ||
                object.Equals(Value, other.Value) ||
                SequenceEqual(Value, other.Value);
        }

        static bool SequenceEqual(object value, object other)
        {
            var seq1 = value as IEnumerable<object>;
            var seq2 = value as IEnumerable<object>;
            var equals = (seq1 != null && seq2 != null ) && Enumerable.SequenceEqual(seq1, seq2);
            return equals;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals((ValueOf<TValue>)obj) ;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(Value);
        }

        public static bool operator ==(ValueOf<TValue> a, ValueOf<TValue> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ValueOf<TValue> a, ValueOf<TValue> b)
        {
            return !(a == b);
        }
        public static implicit operator ValueOf<TValue>(TValue value)
        {
            return ValueOf.From(value);
        }
        public static implicit operator TValue(ValueOf<TValue> valueOf)
        {
            return valueOf.Value;
        }
    }

}
