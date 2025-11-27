using System;
using UnityEngine;

namespace TheFundation.Runtime
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager m_Instance { get; private set; }
        public static FactDictionary Facts { get; } = new();
        public const int _MaxSlots = 10;

        void Awake()
        {
            if (m_Instance) { Destroy(gameObject); return; }
            m_Instance = this;
            DontDestroyOnLoad(gameObject);

            // Langue persistée
            if (Facts.TryGetFact("language", out string lang)) LocalizationManager.SetLanguage(lang);
            else LocalizationManager.SetLanguage("en");

            // Charger save auto
            FactSaveSystem.LoadFromFile(Facts);

            RefreshHasSaveFact();
            
            PlatformizerService.Initialize();
            
            SettingsService.Initialize(_settingsDefinitions);
            
            GoalsService.Initialize(_Goals);

        }

        void OnApplicationPause(bool p) { if (p) FactSaveSystem.SaveToFile(Facts); }
        void OnApplicationQuit() => FactSaveSystem.SaveToFile(Facts);

        // API
        public static void SaveToSlot(int slot) => FactSaveSystem.SaveToSlot(Facts, slot);
        public static void LoadFromSlot(int slot) => FactSaveSystem.LoadFromSlot(Facts, slot);
        public static void DeleteSlot(int slot) => FactSaveSystem.DeleteSlot(slot);
        public static bool HasSaveInSlot(int slot) => FactSaveSystem.SlotExist(slot);

        public static bool AnySaveExists()
        {
            for (int i = 0; i < _MaxSlots; i++)
                if (HasSaveInSlot(i))  return true;
            return false;
        }

        public void RefreshHasSaveFact()
        {
            bool any = AnySaveExists();
            Facts.SetFact("has_save", any, FactDictionary.FactPersistence.Normal);
        }

        [SerializeField] private SettingsDefinitionCollection _settingsDefinitions;
        [SerializeField] private GoalsCollection _Goals;
    }
}