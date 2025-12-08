using System;
using UnityEngine;

namespace CatsClicker.Runtime
{
 [CreateAssetMenu(
        fileName = "UpgradeDefinition",
        menuName = "CatsClicker/Upgrade Definition")]
    public class UpgradeDefinition : ScriptableObject
    {
        #region Publics

        [Header("Identification")]
        public string m_UpgradeId;
        public string m_DisplayName;

        [Header("Type d'effet")]
        public UpgradeEffectType m_EffectType;

        [Header("Coût")]
        public double m_BaseCost;
        public double m_CostGrowthFactor = 1.07d;

        [Header("Effet par niveau")]
        [Tooltip("Par exemple 0.1 = +10% par niveau")]
        public double m_ValuePerLevel = 0.1d;

        [Header("Limites")]
        [Tooltip("0 = pas de limite")]
        public int m_MaxLevel = 0;

        #endregion

        #region API Unity

        private void OnValidate()
        {
            _ClampValues();
        }

        #endregion

        #region Utils (méthodes publics)

        public double ComputeCostForNextLevel(int currentLevel)
        {
            return _ComputeCostForNextLevelInternal(currentLevel);
        }

        public int ClampLevel(int level)
        {
            return _ClampLevelInternal(level);
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _ClampValues()
        {
            if (m_BaseCost < 0d)
            {
                m_BaseCost = 0d;
            }

            if (m_CostGrowthFactor < 1d)
            {
                m_CostGrowthFactor = 1d;
            }

            if (m_ValuePerLevel < 0d)
            {
                m_ValuePerLevel = 0d;
            }

            if (m_MaxLevel < 0)
            {
                m_MaxLevel = 0;
            }
        }

        private double _ComputeCostForNextLevelInternal(int currentLevel)
        {
            return m_BaseCost * Math.Pow(m_CostGrowthFactor, currentLevel);
        }

        private int _ClampLevelInternal(int level)
        {
            if (m_MaxLevel <= 0)
            {
                return level;
            }

            if (level > m_MaxLevel)
            {
                level = m_MaxLevel;
            }

            if (level < 0)
            {
                level = 0;
            }

            return level;
        }

        #endregion

        #region Private and Protected

        // Aucun champ privé pour l'instant

        #endregion
    }
}
