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
            if (definition == null)
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

            return true;
        }

        private double GetCatProductionPerSecondInternal(string catId)
        {
            CatDefinition def = FindCatDefinition(catId);
            if (def == null)
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
                if (def == null || string.IsNullOrWhiteSpace(def.m_catId))
                {
                    continue;
                }

                int level = GetCatLevel(def.m_catId);
                double prod = def.ComputeProductionForLevel(level);
                total += prod;
            }

            double globalMult = GetFact(CatsClickerFacts.m_multGlobalProduction, 1d);
            total *= globalMult;
            
            _totalProductionPerSecond = total;

            SetFact(CatsClickerFacts.m_totalProductionPerSecond, _totalProductionPerSecond,
                FactDictionary.FactPersistence.Normal);
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
                SetFact(key, 0, FactDictionary.FactPersistence.Normal);
            }

            RecomputeTotalProductionPerSecond();
        }

        private CatDefinition FindCatDefinition(string catId)
        {
            foreach (CatDefinition def in m_catsDefinitions)
            {
                if (def != null && def.m_catId == catId)
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
