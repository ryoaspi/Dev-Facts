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
            LocalizationManager.m_Instance.OnLanguageChanged += UpdateText;
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
