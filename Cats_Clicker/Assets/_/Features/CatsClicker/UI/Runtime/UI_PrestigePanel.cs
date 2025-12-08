using CatsClicker.Systems;
using TheFundation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatsClicker.Runtime
{
    public class UiPrestigePanel : FBehaviour
    {
        #region Publics

        [Header("Bindings")]
        public TMP_Text m_PouletsToGainText;
        public Button m_PrestigeButton;

        [Header("Data")]
        public PrestigeSystem m_PrestigeSystem;

        #endregion

        #region API Unity

        private void Start()
        {
            if (m_PrestigeButton != null)
            {
                m_PrestigeButton.onClick.AddListener(_OnPrestigeClicked);
            }
        }

        private void OnDestroy()
        {
            if (m_PrestigeButton != null)
            {
                m_PrestigeButton.onClick.RemoveListener(_OnPrestigeClicked);
            }
        }

        private void Update()
        {
            _Refresh();
        }

        #endregion

        #region Utils (méthodes publics)

        // Rien pour l’instant

        #endregion

        #region Main Methods (méthodes private)

        private void _Refresh()
        {
            if (m_PrestigeSystem == null)
            {
                return;
            }

            bool canPrestige = m_PrestigeSystem.CanPrestige(out int pouletsToGain);

            if (m_PouletsToGainText != null)
            {
                m_PouletsToGainText.text = canPrestige
                    ? $"+{pouletsToGain} poulets"
                    : "Prestige indisponible";
            }

            if (m_PrestigeButton != null)
            {
                m_PrestigeButton.interactable = canPrestige;
            }
        }

        private void _OnPrestigeClicked()
        {
            if (m_PrestigeSystem == null)
            {
                return;
            }

            m_PrestigeSystem.ApplyPrestige();
            _Refresh();
        }

        #endregion

        #region Private and Protected

        // Rien

        #endregion
    }
}
