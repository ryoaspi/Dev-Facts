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
            string spriteName = manager.GetSpriteNameForAction(m_actionName);
            
            string currentScheme = GameManager.m_gameFacts.GetFact<string>("InputScheme");

            string spriteAssetPath = currentScheme switch
            {
                "XboxController" => "Asset/_/Content/Xbox Series/XboxSeriesX_A.png",
                "PlayStationController" => "Asset/_/Content/PS5/PS5_Cross.png",
                "Keyboard" => "Asset/_/Content/Keyboard & Mouse/Dark/Space_Key_Dark.png",
            };
            
            TMP_SpriteAsset spriteAsset = Resources.Load<TMP_SpriteAsset>(spriteAssetPath);
            if (spriteAsset != null)
            {
                m_textComponent.spriteAsset = spriteAsset;
                m_textComponent.text = string.Format(format, $"<sprite name =\"{spriteName}\">");
                
            }

            else
            {
                m_textComponent.text = $"[{m_actionName}]";
                Debug.LogWarning($"SpriteAsset {spriteAssetPath} not found");
            }
            
        }
        
        #endregion
    }
}
