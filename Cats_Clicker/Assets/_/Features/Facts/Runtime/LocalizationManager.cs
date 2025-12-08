using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheFundation.Runtime
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }
        public static string CurrentLanguage { get; private set; } = "en";
        public static event Action OnLanguageChanged;

        static Dictionary<string, string> _texts = new();

        void Awake()
        {
            if (Instance) { Destroy(gameObject); return; }
            Instance = this; DontDestroyOnLoad(gameObject);
            LoadLanguage(CurrentLanguage);
        }

        public static void SetLanguage(string lang) => LoadLanguage(lang);

        public static void LoadLanguage(string language)
        {
            CurrentLanguage = language;
            _texts.Clear();

            var file = Resources.Load<TextAsset>($"Localization/{language}") ??
                       Resources.Load<TextAsset>("Localization/en");
            if (file)
            {
                var data = JsonUtility.FromJson<LocalizationData>(file.text);
                foreach (var it in data.items) _texts[it.key] = it.value;
                GameManager.Facts.SetFact("language", language, FactDictionary.FactPersistence.Persistent);
                OnLanguageChanged?.Invoke();
            }
            else Debug.LogWarning($"Localization file not found for '{language}' nor fallback 'en'.");
        }

        public static string GetText(string key) => _texts.TryGetValue(key, out var v) ? v : $"[{key}]";
    }

    [Serializable] public class LocalizationData { public List<LocalizationItem> items; }
    [Serializable] public class LocalizationItem { public string key; public string value; }
}