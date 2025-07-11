using UnityEngine;

namespace TheFundation.Runtime
{
    public class GameManager : MonoBehaviour
    {
        #region Publics

        public static FactDictionary m_gameFacts = new();
        
        #endregion
        
        
        #region Api Unity

        void Awake()
        {
            LocalizationManager.m_Instance.LoadLanguage("en");
            FactSaveSystem.LoadFromFile(m_gameFacts);
        }

        void OnApplicationQuit()
        {
            FactSaveSystem.SaveToFile(m_gameFacts);
        }
        
        #endregion
        
        
        #region Utils

        public static void SaveGameToSlot(int slot)
        {
            FactSaveSystem.SaveToSlot(m_gameFacts, slot);
        }

        public static void LoadGameFromSlot(int slot)
        {
            FactSaveSystem.LoadFromSlot(m_gameFacts, slot);
        }

        public static void DeleteSaveSlot(int slot)
        {
            FactSaveSystem.DeleteSlot(slot);
        }

        public static bool HasSaveInSlot(int slot)
        {
            return FactSaveSystem.SlotExist(slot);
        }
        #endregion
        
        
        #region Private And Protected
        
        private static FactDictionary _gameFact;
        
        #endregion
    }
}
