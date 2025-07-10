using UnityEngine;

namespace TheFundation.Runtime
{
    public class FBehaviour : MonoBehaviour
    {
        protected bool HasFact<T>(string key, out T value)
        {
            return GameManager.m_gameFacts.FactExist(key, out value);
        }

        protected T GetFact<T>(string key)
        {
            return GameManager.m_gameFacts.GetFact<T>(key);
        }

        protected void SetFact<T>(string key, T value,
            FactDictionary.FactPersistence persistence = FactDictionary.FactPersistence.Normal)
        {
            GameManager.m_gameFacts.SetFact(key, value, persistence);
        }

        protected void RemoveFact(string key)
        {
            GameManager.m_gameFacts.RemoveFact(key);
        }
    }
}
