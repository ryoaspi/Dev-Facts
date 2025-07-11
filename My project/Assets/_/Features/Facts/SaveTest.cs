using System;
using UnityEngine;

namespace TheFundation.Runtime
{
    public class SaveTest : FBehaviour
    {
        private void Start()
        {
           SetFact("playerName", "Thomas", FactDictionary.FactPersistence.Persistent);
           SetFact("gold", 1234, FactDictionary.FactPersistence.Persistent);

           SaveToSlot(0);
           
           Debug.Log("Données sauvegardées dans le slot 0.");
        }
    }
}
