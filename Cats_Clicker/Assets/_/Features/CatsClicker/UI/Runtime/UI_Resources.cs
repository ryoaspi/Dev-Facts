using TheFundation.Runtime;
using TMPro;

namespace CatsClicker.Runtime
{
    public class UI_Resources : FBehaviour
    {
        #region Publics

        public TMP_Text m_CroquettesText;

        #endregion


        #region API Unity

        private void Update()
        {
            _RefreshCroquettes();
        }

        #endregion


        #region Utils (méthodes publics)

        // Rien pour le moment

        #endregion


        #region Main Methods (méthodes private)

        private void _RefreshCroquettes()
        {
            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);
            m_CroquettesText.text = $"{croquettes:0}";
        }

        #endregion


        #region Private and Protected

        // Aucun

        #endregion
    }
}
