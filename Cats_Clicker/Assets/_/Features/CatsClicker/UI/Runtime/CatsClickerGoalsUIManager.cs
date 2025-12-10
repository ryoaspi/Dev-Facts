using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    /// <summary>
    /// Manager qui connecte la GoalsCollection et la TutorialChain 
    /// aux UI existantes GoalPanelUI et GoalChainUI.
    /// </summary>
    public class CatsClickerGoalsUIManager : FBehaviour
    {
        #region Publics

        [Header("Sources")]
        public GoalsCollection m_GoalsCollection;
        public GoalChainDefinition m_TutorialChainDefinition;

        [Header("UI Panels")]
        public GoalPanelUI m_GoalPanelUI;
        public GoalChainUI m_GoalChainUI;

        #endregion


        #region Unity API

        protected override void Start()
        {
            base.Start();
            BindGoalPanel();
            BindGoalChain();
        }

        #endregion


        #region Main Methods (private)

        private void BindGoalPanel()
        {
            if (m_GoalPanelUI == null || m_GoalsCollection == null)
                return;

            // On assigne la collection pour que Start() de GoalPanelUI génère tout automatiquement
            m_GoalPanelUI.m_goalsCollection = m_GoalsCollection;
        }

        private void BindGoalChain()
        {
            if (m_GoalChainUI == null || m_TutorialChainDefinition == null)
                return;

            // On assigne directement la chaîne
            m_GoalChainUI.m_chain = m_TutorialChainDefinition;
        }

        #endregion
    }
}