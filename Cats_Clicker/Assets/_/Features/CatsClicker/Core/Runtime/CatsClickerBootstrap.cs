using CatsClicker.Systems;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    public class CatsClickerBootstrap : FBehaviour
    {
        #region Publics

        [Header("Systems")]
        public ResourceSystem m_ResourceSystem;
        public CatSystem m_CatSystem;
        public UpgradeSystem m_UpgradeSystem;
        public PrestigeSystem m_PrestigeSystem;
        public GoalRewardSystem m_GoalRewardSystem;
        public EventManager m_EventManager;

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

        /// <summary>
        /// Permet un reset global lorsqu'un prestige est appliqué.
        /// </summary>
        public void ResetGameForPrestige()
        {
            _ResetGameForPrestigeInternal();
        }

        #endregion



        #region Main Methods (méthodes private)

        private void _InitializePlatformAndSettings()
        {
            // Initialisation plateforme (mobile, PC…)
            PlatformizerService.Initialize();

            // Initialisation des settings (volume, langue…)
            if (m_SettingsDefinitions != null)
            {
                SettingsService.Initialize(m_SettingsDefinitions);
            }
        }


        private void _EnsureInitialFacts()
        {
            // Croquettes
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_croquettes, out _))
                SetFact(CatsClickerFacts.m_croquettes, 0d);

            // Poissons
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_poissons, out _))
                SetFact(CatsClickerFacts.m_poissons, 0d);

            // Poulets = ressource prestige → persistant
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_poulets, out _))
                SetFact(CatsClickerFacts.m_poulets, 0d, FactDictionary.FactPersistence.Persistent);

            // Production / sec
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_totalProductionPerSecond, out _))
                SetFact(CatsClickerFacts.m_totalProductionPerSecond, 0d);

            // Première ouverture du jeu
            if (!Facts.TryGetFact<bool>(CatsClickerFacts.m_hasLaucheBefore, out _))
                SetFact(CatsClickerFacts.m_hasLaucheBefore, false, FactDictionary.FactPersistence.Persistent);

            // Multiplicateur d'événement
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_eventProductionMultiplier, out _))
                SetFact(CatsClickerFacts.m_eventProductionMultiplier, 1d);
        }


        private void _InitializeSystems()
        {
            // Ordre logique :
            // 1. Systèmes de production
            // 2. Systèmes de progression (cats, upgrades)
            // 3. Prestige
            // 4. Rewards and Events UI

            if (m_ResourceSystem != null)
                m_ResourceSystem.Initialize();

            if (m_CatSystem != null)
                m_CatSystem.Initialize();

            if (m_UpgradeSystem != null)
                m_UpgradeSystem.Initialize();

            if (m_PrestigeSystem != null)
                m_PrestigeSystem.Initialize();

            if (m_GoalRewardSystem != null)
                m_GoalRewardSystem.RebuildCache();

            if (m_EventManager != null)
                Info("EventManager ready.");
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

        // Ajouter ici des champs privés si nécessaire.

        #endregion
    }
}
