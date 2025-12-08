using CatsClicker.Runtime;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Core
{
    public class CatsClickerBootstrap : FBehaviour
    {
        #region Publics

        [Header("Systems")]
        public Systems.ResourceSystem m_ResourceSystem;
        public Systems.CatSystem m_CatSystem;
        public Systems.PrestigeSystem m_PrestigeSystem;

        [Header("Settings")]
        public SettingsDefinitionCollection m_SettingsDefinitions;

        #endregion

        #region API Unity

        private void Awake()
        {
            _InitializePlatformAndSettings();
            _EnsureInitialFacts();
            _InitializeSystems();
        }

        private void Start()
        {
            _InitializeGameState();
        }

        #endregion

        #region Utils (méthodes publics)

        public void ResetGameForPrestige()
        {
            _ResetGameForPrestigeInternal();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _InitializePlatformAndSettings()
        {
            PlatformizerService.Initialize();                       // platform_* dans Facts:contentReference[oaicite:4]{index=4}

            if (m_SettingsDefinitions != null)
            {
                SettingsService.Initialize(m_SettingsDefinitions);   // settings_* dans Facts
            }
        }

        private void _EnsureInitialFacts()
        {
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_croquettes, out _))
            {
                SetFact(CatsClickerFacts.m_croquettes, 0d, FactDictionary.FactPersistence.Normal);
            }

            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_poissons, out _))
            {
                SetFact(CatsClickerFacts.m_poissons, 0d, FactDictionary.FactPersistence.Normal);
            }

            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_poulets, out _))
            {
                // Poulets = ressource de prestige → Persistent
                SetFact(CatsClickerFacts.m_poulets, 0d, FactDictionary.FactPersistence.Persistent);
            }

            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_totalProductionPerSecond, out _))
            {
                SetFact(CatsClickerFacts.m_totalProductionPerSecond, 0d, FactDictionary.FactPersistence.Normal);
            }

            if (!Facts.TryGetFact<bool>(CatsClickerFacts.m_hasLaucheBefore, out _))
            {
                SetFact(CatsClickerFacts.m_hasLaucheBefore, false, FactDictionary.FactPersistence.Persistent);
            }
        }

        private void _InitializeSystems()
        {
            if (m_ResourceSystem != null)
            {
                m_ResourceSystem.Initialize();
            }

            if (m_CatSystem != null)
            {
                m_CatSystem.Initialize();
            }

            if (m_PrestigeSystem != null)
            {
                m_PrestigeSystem.Initialize();
            }
        }

        private void _InitializeGameState()
        {
            Info("CatsClickerBootstrap initialized.");
        }

        private void _ResetGameForPrestigeInternal()
        {
            if (m_PrestigeSystem == null)
            {
                Warning("PrestigeSystemCC is null, cannot reset game.");
                return;
            }

            m_PrestigeSystem.ApplyPrestige();
            _InitializeGameState();
        }

        #endregion

        #region Private and Protected

        // Ajouter des champs privés si besoin

        #endregion
    }
}
