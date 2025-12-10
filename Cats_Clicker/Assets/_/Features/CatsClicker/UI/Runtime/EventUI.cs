using TheFundation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatsClicker.Runtime
{
    public class EventUI : FBehaviour
    {
        #region Publics

        [Header("Bindings")]
        public EventManager m_EventManager;

        public TMP_Text m_LabelText;
        public TMP_Text m_DescriptionText;
        public TMP_Text m_TimerText;

        public Button m_EventButton;
        public GameObject m_PanelRoot;

        #endregion


        #region API Unity

        protected override void Start()
        {
            base.Start();

            if (m_EventButton != null)
            {
                m_EventButton.onClick.AddListener(OnEventClicked);
            }
        }

        private void OnDestroy()
        {
            if (m_EventButton != null)
            {
                m_EventButton.onClick.RemoveListener(OnEventClicked);
            }
        }

        private void Update()
        {
            RefreshUI();
        }

        #endregion


        #region Utils (méthodes publics)

        // Rien pour l’instant

        #endregion


        #region Main Methods (méthodes private)

        private void RefreshUI()
        {
            if (!m_PanelRoot || !m_EventManager)
            {
                return;
            }

            bool hasEvent = m_EventManager.HasActiveEvent();
            m_PanelRoot.SetActive(hasEvent);

            if (!hasEvent)
            {
                return;
            }

            EventDefinition def = m_EventManager.GetActiveEvent();
            if (!def)
            {
                m_PanelRoot.SetActive(false);
                return;
            }

            if (m_LabelText)
            {
                m_LabelText.text = def.m_Label;
            }

            if (m_DescriptionText)
            {
                m_DescriptionText.text = def.m_Description;
            }

            if (m_TimerText)
            {
                float remaining = m_EventManager.GetActiveEventRemainingTime();
                m_TimerText.text = $"{remaining:0.0}s";
            }
        }

        private void OnEventClicked()
        {
            if (!m_EventManager)
            {
                return;
            }
            
            m_EventManager.ResolveAndEndActiveEvent();

            // L’UI appelle la résolution de la récompense, puis termine l’event.
            // On suppose ici que tu exposes _ResolveActiveEventReward et _EndActiveEvent
            // via des méthodes publiques si tu veux une séparation stricte.
            //
            // Pour rester aligné avec ta visibilité actuelle, tu peux ajouter
            // des méthodes publiques dédiées dans EventManager, par exemple :
            //
            // public void ResolveAndEndActiveEvent()
            // {
            //     _ResolveActiveEventReward();
            //     _EndActiveEvent();
            // }
        }

        #endregion


        #region Private and Protected

        // Rien pour l’instant

        #endregion
    }
}
