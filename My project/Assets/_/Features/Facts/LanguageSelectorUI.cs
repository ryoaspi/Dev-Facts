using UnityEngine;

namespace TheFundation.Runtime
{
    public class LanguageSelectorUI : MonoBehaviour
    {
        public void SelectLanguage(string language)
        {
            LocalizationManager.m_Instance.LoadLanguage(language);
        }
    }
}
