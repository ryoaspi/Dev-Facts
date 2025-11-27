using TMPro;
using UnityEngine;

namespace TheFundation.Runtime
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string key;
        TMP_Text _text;

        void Awake()
        {
            _text = GetComponent<TMP_Text>();
            LocalizationManager.OnLanguageChanged += UpdateText;
        }
        void OnDestroy() => LocalizationManager.OnLanguageChanged -= UpdateText;
        void Start() => UpdateText();

        public void SetKey(string k) { key = k; UpdateText(); }
        public void SetFormattedText(params object[] args)
        {
            if (string.IsNullOrEmpty(key)) return;
            _text.text = string.Format(LocalizationManager.GetText(key), args);
        }

        void UpdateText()
        {
            if (!string.IsNullOrEmpty(key))
                _text.text = LocalizationManager.GetText(key);
        }
    }
}