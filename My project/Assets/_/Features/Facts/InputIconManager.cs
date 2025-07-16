using System.Collections.Generic;
using UnityEngine;

namespace TheFundation.Runtime
{
    public class InputIconManager : MonoBehaviour
    {
        #region Publics

        public List<InputIconSet> m_AllIconSets;
        
        #endregion
        
        
        #region Api Unity

        private void Awake()
        {
            string currentScheme = GameManager.m_gameFacts.GetFact<string>("inputScheme");
            var set = m_AllIconSets.Find(x => x.m_inputSchemeName == currentScheme);

            _currentIconMap = new Dictionary<string, Sprite>();
            _currentTextMap = new Dictionary<string, string>();
            
            _currentSpriteNameMap = new Dictionary<string, string>();
            
            if (set != null)
            {
                foreach (var icon in  set.m_icons)
                {
                    _currentIconMap[icon.m_actionName] = icon.m_icon;
                }

                foreach (var text in set.m_texts)
                {
                    _currentTextMap[text.m_actionName] = text.m_displayName;
                }

                foreach (var mapping in set.m_spriteNames)
                {
                    _currentSpriteNameMap[mapping.m_actionName] = mapping.m_spriteName;
                }
            }
            
        }

        #endregion
        
        
        #region Utils

        public Sprite GetIcon(string actionName)
        {
            return _currentIconMap.TryGetValue(actionName, out var icon) ? icon : null;
        }

        public string GetTextLabel(string actionName)
        {
            return _currentTextMap.TryGetValue(actionName, out var label) ? label : "";
        }

        public string GetSpriteNameForAction(string actionName)
        {
            return _currentSpriteNameMap.TryGetValue(actionName, out var spriteName) ? spriteName : "";
        }
        #endregion
        
        
        #region Private And Protected
        
        private Dictionary<string, Sprite> _currentIconMap;
        private Dictionary<string, string> _currentTextMap;
        private Dictionary<string, string> _currentSpriteNameMap;
        
        #endregion
    }
}
