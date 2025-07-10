namespace TheFundation.Runtime
{
    public class GameManager
    {
        #region Publics

        public static FactDictionary m_gameFacts = new();
        
        #endregion
        
        
        #region Api Unity

        void Awake()
        {
            FactSaveSystem.LoadFromFile(m_gameFacts);
        }

        void OnApplicationQuit()
        {
            FactSaveSystem.SaveToFile(m_gameFacts);
        }
        
        #endregion
        
        
        #region Utils

        public void SaveGameToFile()
        {
            FactSaveSystem.SaveToFile(m_gameFacts);
        }

        public void LoadGameFromFile()
        {
            FactSaveSystem.LoadFromFile(m_gameFacts);
        }
        #endregion
        
        
        #region Private And Protected
        
        private static FactDictionary _gameFact;
        
        #endregion
    }
}
