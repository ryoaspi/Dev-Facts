using System.Collections.Generic;
using CatsClicker.Data;
using CatsClicker.Runtime;
using TheFundation.Runtime;

namespace CatsClicker.Systems
{
    public class CatSystem : FBehaviour
    {
        #region Publics

        public List<CatDefinition> m_catsDefinitions = new List<CatDefinition>();
        public GoalChainDefinition m_tutorialChainDefinition;

        #endregion

        #region API Unity

        private void OnValidate()
        {
            RemoveNullDefinitions();
        }

        #endregion

        #region Utils (méthodes publics)

        public void Initialize()
        {
            InitializeInternal();
        }

        public int GetCatLevel(string catId)
        {
            string key = CatsClickerFacts.GetCatLevelKey(catId);
            return GetFact(key, 0);
        }

        public double GetCatProductionPerSecond(string catId)
        {
            return GetCatProductionPerSecondInternal(catId);
        }

        public double GetTotalProductionPerSecond()
        {
            return _totalProductionPerSecond;
        }

        public bool TryBuyLevel(string catId)
        {
            return TryBuyLevelInternal(catId);
        }

        public void ResetAllCatsLevels()
        {
            ResetAllCatsLevelsInternal();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void InitializeInternal()
        {
            RecomputeTotalProductionPerSecond();
            Info("CatSystemCC initialized.");
        }

        private void RemoveNullDefinitions()
        {
            m_catsDefinitions.RemoveAll(def => def == null);
        }

        private bool TryBuyLevelInternal(string catId)
        {
            CatDefinition definition = FindCatDefinition(catId);
            if (!definition)
            {
                Warning($"No CatDefinition found for id: {catId}", this);
                return false;
            }

            int currentLevel = GetCatLevel(catId);
            double cost = definition.ComputeCostForNextLevel(currentLevel);

            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);
            if (croquettes < cost)
            {
                return false;
            }

            croquettes -= cost;
            SetFact(CatsClickerFacts.m_croquettes, croquettes, FactDictionary.FactPersistence.Normal);

            int newLevel = currentLevel + 1;
            string key = CatsClickerFacts.GetCatLevelKey(catId);
            SetFact(key, newLevel, FactDictionary.FactPersistence.Normal);

            RecomputeTotalProductionPerSecond();
            
            // OBJECTIF : premier chat
            if (currentLevel == 0) // donc achat → lvl 1
            {
                GoalChainService.NotifyChain("cc_tutorial_chain", m_tutorialChainDefinition, "cc_buy_first_cat", 1);
                GoalsService.Notify("cc_buy_first_cat", 1);
            }

            // OBJECTIFS : niveaux importants
            if (newLevel >= 5)
            {
                GoalChainService.NotifyChain("cc_tutorial_chain", m_tutorialChainDefinition, "cc_cat_level5", newLevel);
                GoalsService.Notify("cc_cat_level5", newLevel);
            }

            if (newLevel >= 10)
            {
                GoalsService.Notify("cc_cat_level10", newLevel);
            }

            if (newLevel >= 25)
            {
                GoalsService.Notify("cc_cat_level25", newLevel);
            }


            return true;
        }

        private void TryBuyUpgradeInternal(string catId)
        {
            // OBJECTIF : premier upgrade
            GoalChainService.NotifyChain("cc_tutorial_chain", m_tutorialChainDefinition, "cc_buy_first_upgrade", 1);
            GoalsService.Notify("cc_buy_first_upgrade", 1);

            // OBJECTIF : nombre d’upgrades
            GoalsService.Notify("cc_upgrade_5", 1);
            GoalsService.Notify("cc_upgrade_10", 1);

        }

        private double GetCatProductionPerSecondInternal(string catId)
        {
            CatDefinition def = FindCatDefinition(catId);
            if (!def)
            {
                return 0d;
            }

            int level = GetCatLevel(catId);
            return def.ComputeProductionForLevel(level);
        }

        private void RecomputeTotalProductionPerSecond()
        {
            double total = 0d;

            foreach (CatDefinition def in m_catsDefinitions)
            {
                if (!def)
                {
                    continue;
                }

                int level = GetCatLevel(def.m_catId);
                total += def.ComputeProductionForLevel(level);
            }

            double globalMult = GetFact(CatsClickerFacts.m_multGlobalProduction, 1d);

            double eventMult = GetFact(CatsClickerFacts.m_eventProductionMultiplier, 1d);
            total *= globalMult *  eventMult;
            
            _totalProductionPerSecond = total;

            SetFact(CatsClickerFacts.m_totalProductionPerSecond, _totalProductionPerSecond);
        }

        private void ResetAllCatsLevelsInternal()
        {
            foreach (CatDefinition def in m_catsDefinitions)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.m_catId))
                {
                    continue;
                }

                string key = CatsClickerFacts.GetCatLevelKey(def.m_catId);
                SetFact(key, 0);
            }

            RecomputeTotalProductionPerSecond();
        }

        private CatDefinition FindCatDefinition(string catId)
        {
            foreach (CatDefinition def in m_catsDefinitions)
            {
                if (def&& def.m_catId == catId)
                {
                    return def;
                }
            }

            return null;
        }

        #endregion

        #region Private and Protected

        private double _totalProductionPerSecond;

        #endregion
    }
}
