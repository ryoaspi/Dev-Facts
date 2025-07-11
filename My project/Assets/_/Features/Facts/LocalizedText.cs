using System;
using TMPro;
using UnityEngine;

namespace TheFundation.Runtime
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        #region Api Unity

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            if (LocalizationManager.m_Instance != null)
            {
                LocalizationManager.m_Instance.OnLanguageChanged += UpdateText;
            }
            else
            {
                Debug.LogWarning("Localization.m_Instance is null dans LocalizedText.Awake()");
            }
        }
        
        private void OnDestroy()
        {
            if (LocalizationManager.m_Instance != null)
                LocalizationManager.m_Instance.OnLanguageChanged -= UpdateText;
        }

        private void Start()
        {
            UpdateText();
        }

        #endregion
        
        
        #region Utils

        public void SetKey(string key)
        {
            _localizationKey = key;
            UpdateText();
        }

        public void SetFormattedText(params object[] args)
        {
            if (!string.IsNullOrEmpty(_localizationKey))
            {
                string format = LocalizationManager.m_Instance.GetText(_localizationKey);
                _text.text = string.Format(format, args);
            }
        }
        
        #endregion
        
        
        #region Main Methods

        private void UpdateText()
        {
            if (!string.IsNullOrEmpty(_localizationKey))
            {
                _text.text = LocalizationManager.m_Instance.GetText(_localizationKey);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private string _localizationKey;
        
        private TMP_Text _text;
        
        #endregion
    }
}
