using System.Collections.Generic;
using CatsClicker.Runtime;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Systems
{
    public class GoalRewardSystem : FBehaviour
    {
        #region Publics

        [Header("Sources")]
        public GoalsCollection m_goalsCollection;

        [Header("Debug")]
        public bool m_logRewards = true;

        #endregion


        #region API Unity

        private void OnEnable()
        {
            GoalEvents.OnGoalCompleted += _OnGoalCompleted;
        }

        private void OnDisable()
        {
            GoalEvents.OnGoalCompleted -= _OnGoalCompleted;
        }

        protected override void Start()
        {
            base.Start();
            _BuildDictionary();
        }

        #endregion


        #region Utils (méthodes publics)

        public void RebuildCache()
        {
            _BuildDictionary();
        }

        #endregion


        #region Main Methods (méthodes private)

        private void _BuildDictionary()
        {
            _goalsByKey.Clear();

            if (m_goalsCollection == null || m_goalsCollection.m_goals == null)
            {
                Warning("GoalRewardSystem: m_GoalsCollection non assignée ou vide.", this);
                return;
            }

            foreach (GoalDefinition goal in m_goalsCollection.m_goals)
            {
                if (goal == null || string.IsNullOrWhiteSpace(goal.m_Key))
                {
                    continue;
                }

                _goalsByKey[goal.m_Key] = goal;
            }
        }

        private void _OnGoalCompleted(string key)
        {
            if (!_goalsByKey.TryGetValue(key, out GoalDefinition goal) || goal == null)
            {
                return;
            }

            if (goal.m_RewardType == RewardType.None || goal.m_rewardValue <= 0d)
            {
                return;
            }

            double value = goal.m_rewardValue;

            switch (goal.m_RewardType)
            {
                case RewardType.Croquettes:
                    _AddResource(CatsClickerFacts.m_croquettes, value);
                    break;

                case RewardType.Poissons:
                    _AddResource(CatsClickerFacts.m_poissons, value);
                    break;

                case RewardType.Poulets:
                    _AddResource(CatsClickerFacts.m_poulets, value, FactDictionary.FactPersistence.Persistent);
                    break;
            }

            if (m_logRewards)
            {
                InfoDone($"Reward appliquée pour objectif '{key}' : {goal.m_RewardType} +{value}", this);
            }
        }

        private void _AddResource(string factKey, double value, FactDictionary.FactPersistence persistence = FactDictionary.FactPersistence.Normal)
        {
            double current = GetFact(factKey, 0d);
            current += value;
            SetFact(factKey, current, persistence);
        }

        #endregion


        #region Private and Protected

        private readonly Dictionary<string, GoalDefinition> _goalsByKey = new();

        #endregion
    }
}
