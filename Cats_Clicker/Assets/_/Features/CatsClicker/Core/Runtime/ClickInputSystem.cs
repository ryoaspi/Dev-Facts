using TheFundation.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatsClicker.Runtime
{
    public class ClickInputSystem : FBehaviour
    {
        #region Publics

        [Header("Input")]
        public InputActionReference m_clickAction;

        [Header("Click Settings")]
        public double m_baseClickValue = 1d;
        public double m_clickMultiplier = 1d;

        [Header("Tutorial")]
        public GoalChainDefinition m_tutorialChainDefinition;
        
        public EventManager m_eventManager;

        #endregion


        #region API Unity

        private void OnEnable()
        {
            RegisterInput();
        }

        private void OnDisable()
        {
            UnregisterInput();
        }

        #endregion


        #region Utils (méthodes publics)

        public void SetClickMultiplier(double multiplier)
        {
            _clickMultiplierInternal = multiplier;
        }

        public void PerformClick()
        {
            HandleClickPerformed(new InputAction.CallbackContext());
        }

        #endregion


        #region Main Methods (méthodes private)

        private void RegisterInput()
        {
            if (m_clickAction == null || m_clickAction.action == null)
            {
                Warning("ClickInputSystem: m_ClickAction is not set.", this);
                return;
            }

            m_clickAction.action.performed += HandleClickPerformed;
            m_clickAction.action.Enable();
        }

        private void UnregisterInput()
        {
            if (m_clickAction == null || m_clickAction.action == null)
                return;

            m_clickAction.action.performed -= HandleClickPerformed;
            m_clickAction.action.Disable();
        }


        private void HandleClickPerformed(InputAction.CallbackContext context)
        {
            // -------------------------------------------------------------------
            // 🔢 Récupération des valeurs actuelles
            // -------------------------------------------------------------------
            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);

            // 🔥 Multiplicateur provenant des UPGRADES
            double upgradeClickMult = GetFact(CatsClickerFacts.m_multClick, 1d);

            // -------------------------------------------------------------------
            // 🎯 CALCUL DU CLIC (version correcte et complète)
            // -------------------------------------------------------------------
            double value =
                m_baseClickValue *
                m_clickMultiplier *
                _clickMultiplierInternal *
                upgradeClickMult;   // <<🔥 c’est cette partie qui manquait

            croquettes += value;

            SetFact(CatsClickerFacts.m_croquettes, croquettes, FactDictionary.FactPersistence.Normal);

            // -------------------------------------------------------------------
            // 🎯 OBJECTIFS
            // -------------------------------------------------------------------

            // Premier clic
            GoalChainService.NotifyChain("cc_tutorial_chain", m_tutorialChainDefinition, "cc_first_click", 1);
            GoalsService.Notify("cc_first_click", 1);

            // Clics cumulés
            GoalsService.Notify("cc_click100", 1);
            GoalsService.Notify("cc_click1000", 1);

            if (m_eventManager)
            {
                m_eventManager.NotifyPlayerClick();
            }

            InfoInProgress($"Click: +{value} croquettes (total: {croquettes})");
        }

        #endregion


        #region Private and Protected

        private double _clickMultiplierInternal = 1d;

        #endregion
    }
}
