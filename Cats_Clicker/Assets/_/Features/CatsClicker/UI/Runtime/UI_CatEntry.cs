using CatsClicker.Data;
using CatsClicker.Systems;
using TheFundation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatsClicker.Runtime
{
public class UiCatEntry : FBehaviour
    {
        #region Publics

        [Header("Bindings")]
        public TMP_Text m_NameText;
        public TMP_Text m_LevelText;
        public TMP_Text m_ProductionText;
        public TMP_Text m_CostText;
        public Button m_BuyButton;

        [Header("Data")]
        public CatDefinition m_CatDefinition;
        public CatSystem m_CatSystem;

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

        public void SetData(CatDefinition definition, CatSystem system)
        {
            m_CatDefinition = definition;
            m_CatSystem = system;
            _Refresh();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _Refresh()
        {
            if (m_CatDefinition == null || m_CatSystem == null)
            {
                return;
            }

            int level = m_CatSystem.GetCatLevel(m_CatDefinition.m_catId);
            double prod = m_CatSystem.GetCatProductionPerSecond(m_CatDefinition.m_catId);
            double cost = m_CatDefinition.ComputeCostForNextLevel(level);

            if (!m_NameText)
            {
                m_NameText.text = m_CatDefinition.m_DisplayName;
            }

            if (!m_LevelText)
            {
                m_LevelText.text = $"Lv {level}";
            }

            if (!m_ProductionText)
            {
                m_ProductionText.text = $"{prod:0.##}/s";
            }

            if (!m_CostText)
            {
                m_CostText.text = $"{cost:0}";
            }
        }

        private void _OnBuyClicked()
        {
            if (m_CatSystem == null || m_CatDefinition == null)
            {
                return;
            }

            bool success = m_CatSystem.TryBuyLevel(m_CatDefinition.m_catId);
            if (!success)
            {
                // On pourrait jouer un feedback d'erreur ici (son, shake, etc.)
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
