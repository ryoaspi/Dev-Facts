using System;
using CatsClicker.Runtime;
using UnityEngine;

namespace CatsClicker.Data
{
    [CreateAssetMenu(
        fileName = "CatDefinition",
        menuName = "CatsClicker/Cat Definition")]
    public class CatDefinition : ScriptableObject
    {
        #region Publics

        public string m_catId;
        public string m_DisplayName;
        public CatRarity m_Rarity;

        [Header("Production")]
        public double m_BaseProductionPerSecond;

        [Header("Cost")]
        public double m_BaseCost;
        public double m_CostGrowthFactor = 1.07d;

        [Header("Level Multipliers")]
        public double m_LevelMultiplier = 1.15d;

        [Header("Milestones")]
        public int m_MilestoneLevel10 = 10;
        public int m_MilestoneLevel25 = 25;
        public int m_MilestoneLevel50 = 50;

        public double m_MilestoneMultiplier10 = 2.0d;
        public double m_MilestoneMultiplier25 = 3.0d;
        public double m_MilestoneMultiplier50 = 5.0d;

        #endregion

        #region API Unity

        private void OnValidate()
        {
            _ClampValues();
        }

        #endregion

        #region Utils (méthodes publics)

        public double ComputeProductionForLevel(int level)
        {
            return _ComputeProductionForLevelInternal(level);
        }

        public double ComputeCostForNextLevel(int currentLevel)
        {
            return _ComputeCostForNextLevelInternal(currentLevel);
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _ClampValues()
        {
            if (m_BaseProductionPerSecond < 0d)
            {
                m_BaseProductionPerSecond = 0d;
            }

            if (m_BaseCost < 0d)
            {
                m_BaseCost = 0d;
            }

            if (m_CostGrowthFactor < 1d)
            {
                m_CostGrowthFactor = 1d;
            }
        }

        private double _ComputeProductionForLevelInternal(int level)
        {
            if (level <= 0)
            {
                return 0d;
            }

            double baseValue = m_BaseProductionPerSecond * Math.Pow(m_LevelMultiplier, level - 1);
            double milestoneMultiplier = _GetMilestoneMultiplier(level);
            return baseValue * milestoneMultiplier;
        }

        private double _ComputeCostForNextLevelInternal(int currentLevel)
        {
            return m_BaseCost * Math.Pow(m_CostGrowthFactor, currentLevel);
        }

        private double _GetMilestoneMultiplier(int level)
        {
            double multiplier = 1d;

            if (level >= m_MilestoneLevel10)
            {
                multiplier *= m_MilestoneMultiplier10;
            }

            if (level >= m_MilestoneLevel25)
            {
                multiplier *= m_MilestoneMultiplier25;
            }

            if (level >= m_MilestoneLevel50)
            {
                multiplier *= m_MilestoneMultiplier50;
            }

            return multiplier;
        }

        #endregion

        #region Private and Protected

        // Rien pour l’instant

        #endregion
    }
}
