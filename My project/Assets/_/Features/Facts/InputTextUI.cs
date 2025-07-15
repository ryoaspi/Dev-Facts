using TMPro;
using UnityEngine;

namespace TheFundation.Runtime
{
    public class InputTextUI : MonoBehaviour
    {
        #region Publics
        
        public string m_actionName;
        public TMP_Text m_textComponent;
        public string format = "Appuie sur {0}";
        
        #endregion
        
        
        #region Api Unity
        
        private void OnEnable()
        {
            InputIconEvents.OnInputSchemeChanged += Refresh;
        }
        
        void Start()
        {
            var manager = FindObjectOfType<InputIconManager>();
            if (manager != null)
            {
                string inputLabel = manager.GetTextLabel(m_actionName);
                m_textComponent.text = string.Format(format, inputLabel);
            }
        }
        
        private void OnDisable()
        {
            InputIconEvents.OnInputSchemeChanged -= Refresh;
        }
        #endregion
        
        
        #region Main Methods
        
        private void Refresh()
        {
            var manager = FindObjectOfType<InputIconManager>();
            string label = manager.GetTextLabel(m_actionName);
            
            m_textComponent.text = string.Format(format, label);
            m_textComponent.text = string.Format(format, $"<sprite name=\"{label}\">");
        }
        
        #endregion
    }
}
