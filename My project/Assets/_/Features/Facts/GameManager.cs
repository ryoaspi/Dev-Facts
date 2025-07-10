using UnityEngine;

namespace TheFundation.Runtime
{
    public class GameManager
    {
        #region Publics

        public static FactDictionary m_gameFacts
        {
            get
            {
                if (_gameFact != null) return _gameFact;
                _gameFact = new FactDictionary();
                return _gameFact;
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        private static FactDictionary _gameFact;
        
        #endregion
    }
}
