using CatsClicker.Runtime;
using TheFundation.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatsClicker.Core
{
    public class ClickInputSystem : FBehaviour
    {
        #region Publics

        [Header("Input")]
        public InputActionReference m_ClickAction;

        [Header("Click Settings")]
        public double m_BaseClickValue = 1d;
        public double m_ClickMultiplier = 1d;

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

        #endregion

        #region Main Methods (méthodes private)

        private void RegisterInput()
        {
            if (m_ClickAction == null || m_ClickAction.action == null)
            {
                Warning("ClickInputSystemCC: m_ClickAction is not set.", this);
                return;
            }

            m_ClickAction.action.performed += HandleClickPerformed;
            m_ClickAction.action.Enable();
        }

        private void UnregisterInput()
        {
            if (m_ClickAction == null || m_ClickAction.action == null)
            {
                return;
            }

            m_ClickAction.action.performed -= HandleClickPerformed;
            m_ClickAction.action.Disable();
        }

        private void HandleClickPerformed(InputAction.CallbackContext context)
        {
            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);

            double upgradeClickMult = GetFact(CatsClickerFacts.m_multClick, 1d);

            double value = m_BaseClickValue * m_ClickMultiplier * _clickMultiplierInternal;
            croquettes += value;

            SetFact(CatsClickerFacts.m_croquettes, croquettes, FactDictionary.FactPersistence.Normal);

            InfoInProgress($"Click: +{value} croquettes (total: {croquettes})");
        }

        #endregion

        #region Private and Protected

        private double _clickMultiplierInternal = 1d;

        #endregion
    }
}
