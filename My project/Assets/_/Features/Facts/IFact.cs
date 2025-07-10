namespace TheFundation.Runtime
{
    public interface IFact
    {
        object GetObjectValue();
        void SetObjectValue(object value);
        bool IsPersistent { get; set; }
        
    }
}

