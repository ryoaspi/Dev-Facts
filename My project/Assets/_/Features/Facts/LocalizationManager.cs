using System;
using System.Collections.Generic;
using UnityEngine;


namespace TheFundation.Runtime
{
    public class LocalizationManager : MonoBehaviour
    {
        #region Publics

        public static LocalizationManager m_Instance;

        public string CurrentLanguage { get; private set; } = "en";
        
        public event Action OnLanguageChanged;

        #endregion
        
        
        #region Api Unity

        private void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadLanguage(CurrentLanguage);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        #endregion
        
        
        #region Utils

        public void LoadLanguage(string language)
        {
            CurrentLanguage = language;
            _localizedTexts.Clear();
            
            //Exemple simple: charger un fichier JSON ou ScriptableObject contenant les traductions
            TextAsset langFile = Resources.Load<TextAsset>($"Localization/{language}");
            if (langFile != null)
            {
                LocalizationData data = JsonUtility.FromJson<LocalizationData>(langFile.text);
                foreach (var item in data.items)
                {
                    _localizedTexts[item.key] = item.value;
                }
                
                OnLanguageChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning($"Langue {language} introuvable");
            }
        }

        public string GetText(string key)
        {
            return _localizedTexts.TryGetValue(key, out var value) ? value : $"[{key}]";
        }
        
        #endregion
        
        
        #region Private And Protected
        
        private Dictionary<string, string> _localizedTexts = new ();
        
        #endregion
    }

    [Serializable]
    public class LocalizationData
    {
        public List<LocalizationItem> items;
    }

    [Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;
    }
}
