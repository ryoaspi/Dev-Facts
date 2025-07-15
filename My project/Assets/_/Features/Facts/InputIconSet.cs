using System.Collections.Generic;
using UnityEngine;

namespace TheFundation.Runtime
{
    [CreateAssetMenu(fileName = "InputIconSet", menuName = "Input/IconSet")]
    public class InputIconSet : ScriptableObject
    {
        public string m_inputSchemeName;
        public List<InputIconMapping> m_icons = new();
        public List<InputTextMapping> m_texts = new();
    }

    [System.Serializable]
    public class InputIconMapping
    {
        public string m_actionName;
        public Sprite m_icon;
    }
    
    [System.Serializable]
    public class InputTextMapping
    {
        public string m_actionName; // Ex : "Jump"
        public string m_displayName; // Ex : "A" , "X", "Space", etc.
    }
}
