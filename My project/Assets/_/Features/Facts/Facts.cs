using System;

namespace TheFundation.Runtime.Data
{
    public class Facts<T> : IFact
    {
        public T Value;
        
        public Type ValueType { get; }

        public Facts(T value, bool isPersistent = false)
        {
            Value = value;
            IsPersistent = isPersistent;
            ValueType = value.GetType();
        }
        
        public object GetObjectValue() => Value;

        public void SetObjectValue(object value)
        {
            if (value is T castedValue)
                Value = castedValue;
            else
                throw new ArgumentException($"Cannot cast {value.GetType()} to {typeof(T)}", nameof(value));
        }
        
        public bool IsPersistent { get; set; }


    }
}