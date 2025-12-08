using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TheFundation.Runtime
{
    /// <summary>
    /// Affiche une étape de chaîne (GoalDefinition).
    /// - Localisation automatique
    /// - Mise à jour via GoalEvents
    /// </summary>
    public class GoalChainStepUI : FBehaviour
    {
        #region Publics (Bindings)

        [Header("Données")]
        public GoalDefinition m_goal;

        [Header("UI")]
        public TMP_Text m_label;
        public TMP_Text m_description;
        public TMP_Text m_progressText;
        public Slider m_progressBar;

        public GameObject m_iconCompleted;
        public GameObject m_iconCurrent;
        public GameObject m_iconLocked;

        #endregion


        #region Private

        private string _goalKey;

        #endregion


        #region Unity

        protected override void Start()
        {
            base.Start();

            if (m_goal == null)
            {
                Debug.LogError("[GoalChainStepUI] GoalDefinition manquant.", this);
                enabled = false;
                return;
            }

            _goalKey = m_goal.m_Key;
            ApplyLocalization();
            RefreshUI();

            GoalEvents.OnGoalProgress += OnProgress;
            GoalEvents.OnGoalCompleted += OnCompleted;
        }

        private void OnDestroy()
        {
            GoalEvents.OnGoalProgress -= OnProgress;
            GoalEvents.OnGoalCompleted -= OnCompleted;
        }

        #endregion


        #region UI Logic

        private void ApplyLocalization()
        {
            if (m_label)
            {
                if (!string.IsNullOrEmpty(m_goal.m_LabelKey))
                    m_label.text = LocalizationManager.GetText(m_goal.m_LabelKey);
                else
                    m_label.text = m_goal.m_Label;
            }

            if (m_description)
            {
                if (!string.IsNullOrEmpty(m_goal.m_DescriptionKey))
                    m_description.text = LocalizationManager.GetText(m_goal.m_DescriptionKey);
                else
                    m_description.text = m_goal.m_Description;
            }
        }


        public void RefreshUI()
        {
            bool completed = GoalsService.IsCompleted(_goalKey);
            int progress = GoalsService.GetProgress(_goalKey);

            // Completed / Current / Locked handled by parent GoalChainUI
            // Ici on affiche juste la progression

            if (m_progressText && m_goal.m_TargetValue > 1)
                m_progressText.text = $"{progress} / {m_goal.m_TargetValue}";

            if (m_progressBar && m_goal.m_TargetValue > 1)
                m_progressBar.value = (float)progress / m_goal.m_TargetValue;
        }

        #endregion


        #region Events

        private void OnProgress(string key, int value)
        {
            if (key == _goalKey)
                RefreshUI();
        }

        private void OnCompleted(string key)
        {
            if (key == _goalKey)
                RefreshUI();
        }

        #endregion
    }
}
