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
        }
        
        public object GetObjectValue() => Value;

        public void SetObjectValue(object value)
        {
            throw new NotImplementedException();
        }
        
        public bool IsPersistent { get; set; }


    }
}
