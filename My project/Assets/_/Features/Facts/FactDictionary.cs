using System;
using System.Collections.Generic;
using TheFundation.Runtime.Data;

namespace TheFundation.Runtime
{
    public class FactDictionary
    {
        
        #region Publics
        
        public Dictionary<string, IFact> m_AllFacts => _facts;
        
        public enum FactPersistence
        {
            Normal,
            Persistent
        }
        
        #endregion
        
        
        #region Utils

        public bool TryGetFact<T>(string key, out T value)
        {
            if (_facts.TryGetValue(key, out var fact) && fact is Facts<T> typedFact)
            {
                value = typedFact.Value;
                return true;
            }
            
            value = default;
            return false;
        }
        
        public bool FactExist<T>(string key, out T value)
        { 
            if (_facts.TryGetValue(key, out var fact) && fact is Facts<T> typedFact)
            {
                value = typedFact.Value;
                return true;
            }
            
            value = default;
            return false;
        }
        
        public void RemoveFact(string key)
        {
            _facts.Remove(key);
        }

        public T GetFact<T>(string key)
        {
            if (!_facts.TryGetValue(key, out var fact)) throw new KeyNotFoundException("No Fact");
            if (fact is not Facts<T> typedFact) throw new InvalidCastException("Fact is not of type T");
            
            return typedFact.Value;
        }

        public void SetFact<T>(string key, T value, FactPersistence persistence)
        {
            if (_facts.TryGetValue(key, out var existingFact))
            {
                if (existingFact is Facts<T> typedFact)
                {
                    typedFact.Value = value;
                    typedFact.IsPersistent = persistence == FactPersistence.Persistent;
                }
                else
                {
                    throw new InvalidCastException("Fact exist but with the wrong type");
                }
            }
            else
            {
                bool isPersistent = persistence == FactPersistence.Persistent;
                _facts[key] = new Facts<T>(value, isPersistent);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        private Dictionary<string, IFact> _facts = new();
        
        #endregion
        
    }
}
