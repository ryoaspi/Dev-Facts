using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFundation.Runtime
{
    public class FBehaviour : MonoBehaviour
    {
        #region Publics (Services & UI Definitions)

        /// <summary>
        /// Définition UI optionnelle pour un réglage
        /// (slider / toggle / dropdown).
        /// 
        /// Si renseignée dans l’inspector, FBehaviour
        /// tentera automatiquement de binder le composant UI.
        /// </summary>
        [Serializable]
        public class SettingUIDefinitionData
        {
            public SettingDefinition m_Definition;
        }

        [Header("Settings UI (optionnel)")]
        public SettingUIDefinitionData m_settingDefinition;

        /// <summary>
        /// Accès direct au système de Facts V1.
        /// </summary>
        public FactDictionary Facts => GameManager.Facts;

        #endregion



        #region API Unity

        protected virtual void Start()
        {
            // Auto-binder des réglages UI
            AutoBindSettingIfNeeded();
        }

        #endregion



        #region Utils (Méthodes publiques)

        public T GetFact<T>(string key, T defaultValue = default)
        {
            return Facts.TryGetFact(key, out T v) ? v : defaultValue;
        }

        public void SetFact<T>(string key, T value,
            FactDictionary.FactPersistence persistence = FactDictionary.FactPersistence.Normal)
        {
            Facts.SetFact(key, value, persistence);
        }

        public void SaveFacts() => FactSaveSystem.SaveToFile(Facts);
        public void LoadFacts() => FactSaveSystem.LoadFromFile(Facts);

        public void DebugFact(string key)
        {
            if (Facts.TryGetFact<object>(key, out object v))
                Debug.Log($"[Fact] {key} = {v}");
            else
                Debug.Log($"[Fact] {key} n'existe pas.");
        }

        #endregion



        #region Main Methods (Méthodes privées)

        private void AutoBindSettingIfNeeded()
        {
            var def = m_settingDefinition?.m_Definition;
            if (def == null)
                return;

            // Bind Slider
            if (TryGetComponent<Slider>(out var slider))
            {
                SliderBinderLogic.Bind(slider, def);
                ApplyPlatformVisibility(def);
                ApplyLocalization(def);
                return;
            }

            // Bind Toggle
            if (TryGetComponent<Toggle>(out var toggle))
            {
                ToggleBinderLogic.Bind(toggle, def);
                ApplyPlatformVisibility(def);
                ApplyLocalization(def);
                return;
            }

            // Bind Dropdown
            if (TryGetComponent<Dropdown>(out var dropdown))
            {
                DropdownBinderLogic.Bind(dropdown, def);
                ApplyPlatformVisibility(def);
                ApplyLocalization(def);
                return;
            }
        }

        /// <summary>
        /// Masque l'UI selon la plateforme (Desktop / Mobile / Console).
        /// </summary>
        private void ApplyPlatformVisibility(SettingDefinition def)
        {
            string family = GetFact("platform_family", "Desktop");

            bool show =
                (family == "Desktop" && def.m_showOnDesktop) ||
                (family == "Mobile" && def.m_showOnMobile) ||
                (family == "Console" && def.m_showOnConsole);

            gameObject.SetActive(show);
        }

        /// <summary>
        /// Applique la localisation sur le label et la description
        /// (si présents dans l’inspector).
        /// </summary>
        private void ApplyLocalization(SettingDefinition def)
        {
            if (m_label)
            {
                if (!string.IsNullOrEmpty(def.m_labelKey))
                    m_label.text = LocalizationManager.GetText(def.m_labelKey);
                else
                    m_label.text = def.m_label;
            }

            if (m_description)
            {
                if (!string.IsNullOrEmpty(def.m_descriptionKey))
                    m_description.text = LocalizationManager.GetText(def.m_descriptionKey);
                else
                    m_description.text = def.m_description;
            }
            
        }

        protected void GoalNotify(string key, int amount = 1)
        {
            GoalsService.Notify(key, amount);
        }

        #endregion



        #region Private & Protected Fields

        [SerializeField] private TMP_Text m_label;
        [SerializeField] private TMP_Text m_description;

        #endregion
    }
}
