using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheFundation.Runtime
{
    public class InputIconUI : MonoBehaviour
    {
        #region Publics
        
        public string actionName;
        public Image IconImage;
        
        #endregion
        
        
        #region Api Unity

        void Start()
        {
            var manager = FindObjectOfType<InputIconManager>();
            if (manager != null)
            {
                IconImage.sprite = manager.GetIcon(actionName);
            }            
        }

        #endregion
        
        
        #region Main Methods


        
        #endregion
    }
}
