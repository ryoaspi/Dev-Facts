using System.Collections.Generic;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    public class UpgradeSystem : FBehaviour
    {
        #region Publics

        [Header("Upgrades disponibles")]
        public List<UpgradeDefinition> m_Upgrades = new();

        [Header("Tutorial")]
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

        public int GetUpgradeLevel(string upgradeId)
        {
            string key = CatsClickerFacts.GetUpgradeLevelKey(upgradeId);
            return GetFact(key, 0);
        }

        public bool TryBuyUpgrade(string upgradeId)
        {
            return TryBuyUpgradeInternal(upgradeId);
        }

        public void RecomputeAllEffects()
        {
            RecomputeAllEffectsInternal();
        }

        #endregion


        #region Main Methods (méthodes private)

        private void InitializeInternal()
        {
            // Init multiplicateurs si absents
            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_multGlobalProduction, out _))
                SetFact(CatsClickerFacts.m_multGlobalProduction, 1d);

            if (!Facts.TryGetFact<double>(CatsClickerFacts.m_multClick, out _))
                SetFact(CatsClickerFacts.m_multClick, 1d);

            RecomputeAllEffectsInternal();
            Info("UpgradeSystem initialized.");
        }


        private void RemoveNullDefinitions()
        {
            m_Upgrades.RemoveAll(u => u == null);
        }


        private bool TryBuyUpgradeInternal(string upgradeId)
        {
            UpgradeDefinition def = FindUpgrade(upgradeId);
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
                return false;

            double cost = def.ComputeCostForNextLevel(currentLevel);
            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);

            if (croquettes < cost)
                return false;

            // Paiement
            SetFact(CatsClickerFacts.m_croquettes, croquettes - cost);

            // Nouveau niveau
            SetFact(levelKey, nextLevel);

            // Recalcul des effets
            RecomputeAllEffectsInternal();

            // --------------------------------------------------------------------
            // 🎯 OBJECTIFS (Goals + Tutorial Chain)
            // --------------------------------------------------------------------

            // Premier upgrade
            if (currentLevel == 0)
            {
                GoalChainService.NotifyChain("cc_tutorial_chain", m_tutorialChainDefinition, "cc_buy_first_upgrade", 1);
                GoalsService.Notify("cc_buy_first_upgrade", 1);
            }

            // Upgrades cumulés
            GoalsService.Notify("cc_upgrade_5", 1);
            GoalsService.Notify("cc_upgrade_10", 1);

            // Objectif multiplicateur global x2
            double globalMult = GetFact(CatsClickerFacts.m_multGlobalProduction, 1d);
            if (globalMult >= 2.0)
                GoalsService.Notify("cc_mult2", 2);

            InfoInProgress($"Upgrade bought: {upgradeId} → level {nextLevel}");
            return true;
        }


        private void RecomputeAllEffectsInternal()
        {
            double globalProdMult = 1d;
            double clickMult = 1d;

            foreach (UpgradeDefinition def in m_Upgrades)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.m_UpgradeId))
                    continue;

                string levelKey = CatsClickerFacts.GetUpgradeLevelKey(def.m_UpgradeId);
                int level = GetFact(levelKey, 0);

                if (level <= 0)
                    continue;

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

            SetFact(CatsClickerFacts.m_multGlobalProduction, globalProdMult);
            SetFact(CatsClickerFacts.m_multClick, clickMult);
        }


        private UpgradeDefinition FindUpgrade(string upgradeId)
        {
            foreach (var def in m_Upgrades)
            {
                if (def != null && def.m_UpgradeId == upgradeId)
                    return def;
            }

            return null;
        }

        #endregion


        #region Private and Protected

        // Aucun champ privé

        #endregion
    }
}
