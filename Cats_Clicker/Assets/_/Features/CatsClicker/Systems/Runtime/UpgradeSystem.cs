using System.Collections.Generic;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
 public class UpgradeSystem : FBehaviour
    {
        #region Publics

        [Header("Upgrades disponibles")]
        public List<UpgradeDefinition> m_Upgrades = new List<UpgradeDefinition>();

        #endregion

        #region API Unity

        private void OnValidate()
        {
            _RemoveNullDefinitions();
        }

        #endregion

        #region Utils (méthodes publics)

        public void Initialize()
        {
            _InitializeInternal();
        }

        public int GetUpgradeLevel(string upgradeId)
        {
            string key = CatsClickerFacts.GetUpgradeLevelKey(upgradeId);
            return GetFact(key, 0);
        }

        public bool TryBuyUpgrade(string upgradeId)
        {
            return _TryBuyUpgradeInternal(upgradeId);
        }

        public void RecomputeAllEffects()
        {
            _RecomputeAllEffectsInternal();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _InitializeInternal()
        {
            // Init des multiplicateurs si absents
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_multGlobalProduction, out _))
            {
                SetFact(CatsClickerFacts.m_multGlobalProduction, 1d, FactDictionary.FactPersistence.Normal);
            }

            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_multClick, out _))
            {
                SetFact(CatsClickerFacts.m_multClick, 1d, FactDictionary.FactPersistence.Normal);
            }

            _RecomputeAllEffectsInternal();
            Info("UpgradeSystem initialized.");
        }

        private void _RemoveNullDefinitions()
        {
            m_Upgrades.RemoveAll(u => u == null);
        }

        private bool _TryBuyUpgradeInternal(string upgradeId)
        {
            UpgradeDefinition def = _FindUpgrade(upgradeId);
            if (def == null)
            {
                Warning($"No UpgradeDefinition found for id: {upgradeId}", this);
                return false;
            }

            string levelKey = CatsClickerFacts.GetUpgradeLevelKey(upgradeId);
            int currentLevel = GetFact(levelKey, 0);
            int nextLevel = currentLevel + 1;

            nextLevel = def.ClampLevel(nextLevel);
            if (nextLevel == currentLevel)
            {
                // déjà au max
                return false;
            }

            double cost = def.ComputeCostForNextLevel(currentLevel);
            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);

            if (croquettes < cost)
            {
                return false;
            }

            croquettes -= cost;
            SetFact(CatsClickerFacts.m_croquettes, croquettes, FactDictionary.FactPersistence.Normal);

            SetFact(levelKey, nextLevel, FactDictionary.FactPersistence.Normal);

            _RecomputeAllEffectsInternal();

            InfoInProgress($"Upgrade bought: {upgradeId} → level {nextLevel}");
            return true;
        }

        private void _RecomputeAllEffectsInternal()
        {
            double globalProdMult = 1d;
            double clickMult = 1d;

            foreach (UpgradeDefinition def in m_Upgrades)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.m_UpgradeId))
                {
                    continue;
                }

                string levelKey = CatsClickerFacts.GetUpgradeLevelKey(def.m_UpgradeId);
                int level = GetFact(levelKey, 0);

                if (level <= 0)
                {
                    continue;
                }

                double factor = 1d + def.m_ValuePerLevel * level;

                switch (def.m_EffectType)
                {
                    case UpgradeEffectType.GlobalProductionMultiplier:
                        globalProdMult *= factor;
                        break;

                    case UpgradeEffectType.ClickMultiplier:
                        clickMult *= factor;
                        break;
                }
            }

            SetFact(CatsClickerFacts.m_multGlobalProduction, globalProdMult, FactDictionary.FactPersistence.Normal);
            SetFact(CatsClickerFacts.m_multClick, clickMult, FactDictionary.FactPersistence.Normal);
        }

        private UpgradeDefinition _FindUpgrade(string upgradeId)
        {
            foreach (UpgradeDefinition def in m_Upgrades)
            {
                if (def != null && def.m_UpgradeId == upgradeId)
                {
                    return def;
                }
            }

            return null;
        }

        #endregion

        #region Private and Protected

        // Pas de champs privés pour l’instant

        #endregion
    }
}
