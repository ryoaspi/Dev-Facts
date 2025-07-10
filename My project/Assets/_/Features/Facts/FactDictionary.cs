using System;
using System.Collections.Generic;
using TheFundation.Runtime.Data;

namespace TheFundation.Runtime
{
    public class FactDictionary : FBehaviour
    {
        public Dictionary<string, IFact> AllFacts => _facts;
        private Dictionary<string, IFact> _facts;
        
        #region Main API
        
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

        public void RemoveFact<T>(string key)
        {
            
        }

        public T GetFact<T>(string key)
        {
            if (!_facts.TryGetValue(key, out var fact)) throw new KeyNotFoundException("No Fact with key " + key + " found");
            if (fact is Facts<T> typedFact) throw new InvalidCastException("Fact exists but with the wrong type");
            
            return typedFact.Value;
        }

        public void SetFact<T>(string key, T value, FactPersistence persistence)
        {
            if (_facts.TryGetValue(key, out var existingFact))
            {
                if (existingFact is Facts<T> typedfact)
                {
                    typedfact.Value = value;
                    typedfact.IsPersistent = persistence == FactPersistence.Persistent;
                }
                else
                {
                    throw new InvalidCastException(" Fact exists but with the wrong type");
                }
            }
            else
            {
                bool isPersistent = persistence == FactPersistence.Persistent;
                _facts[key] = new Facts<T>(value, isPersistent);
            }
        }
        
        
        #endregion
    }
}
