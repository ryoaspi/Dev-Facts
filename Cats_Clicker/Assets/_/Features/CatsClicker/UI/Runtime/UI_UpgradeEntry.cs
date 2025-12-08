using TheFundation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatsClicker.Runtime
{
    public class UiUpgradeEntry : FBehaviour
    {
        #region Publics

        [Header("Bindings")]
        public TMP_Text m_NameText;
        public TMP_Text m_LevelText;
        public TMP_Text m_EffectText;
        public TMP_Text m_CostText;
        public Button m_BuyButton;

        [Header("Data")]
        public UpgradeDefinition m_UpgradeDefinition;
        public UpgradeSystem m_UpgradeSystem;

        #endregion

        #region API Unity

        private void Start()
        {
            _Refresh();
            if (m_BuyButton != null)
            {
                m_BuyButton.onClick.AddListener(_OnBuyClicked);
            }
        }

        private void OnDestroy()
        {
            if (m_BuyButton != null)
            {
                m_BuyButton.onClick.RemoveListener(_OnBuyClicked);
            }
        }

        private void Update()
        {
            _Refresh();
        }

        #endregion

        #region Utils (méthodes publics)

        public void SetData(UpgradeDefinition definition, UpgradeSystem system)
        {
            m_UpgradeDefinition = definition;
            m_UpgradeSystem = system;
            _Refresh();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _Refresh()
        {
            if (m_UpgradeDefinition == null || m_UpgradeSystem == null)
            {
                return;
            }

            int level = m_UpgradeSystem.GetUpgradeLevel(m_UpgradeDefinition.m_UpgradeId);
            double cost = m_UpgradeDefinition.ComputeCostForNextLevel(level);

            if (m_NameText != null)
            {
                m_NameText.text = m_UpgradeDefinition.m_DisplayName;
            }

            if (m_LevelText != null)
            {
                m_LevelText.text = $"Lv {level}";
            }

            if (m_EffectText != null)
            {
                string effectUnit = m_UpgradeDefinition.m_ValuePerLevel * 100d + "%";
                m_EffectText.text = $"+{effectUnit} / niveau";
            }

            if (m_CostText != null)
            {
                m_CostText.text = $"{cost:0}";
            }
        }

        private void _OnBuyClicked()
        {
            if (m_UpgradeSystem == null || m_UpgradeDefinition == null)
            {
                return;
            }

            bool success = m_UpgradeSystem.TryBuyUpgrade(m_UpgradeDefinition.m_UpgradeId);
            if (!success)
            {
                return;
            }

            _Refresh();
        }

        #endregion

        #region Private and Protected

        // Rien pour l’instant

        #endregion
    }
}
