using UnityEngine;

namespace TheFundation.Runtime
{
    public class LoadTest : FBehaviour
    {
        
        void Start()
        {
            LoadFromSlot(0);
            
            string name = GetFact<string>("playerName");
            int gold = GetFact<int>("gold");
            string race = GetFact<string>("Race");
            
            Debug.Log($"[Chargement terminé] Nom : {name}, Gold : {gold}, Race : {race}");
        }

    }
}
