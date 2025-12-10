using System.Collections.Generic;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    public class EventManager : FBehaviour
    {
        #region Publics

        [Header("Definitions")]
        public List<EventDefinition> m_Events = new();

        [Header("Debug")]
        public bool m_LogEvents = true;

        #endregion


        #region API Unity

        private void Update()
        {
            UpdateTimedEvents(Time.deltaTime);
            UpdateActiveEvent(Time.deltaTime);
        }

        #endregion


        #region Utils (méthodes publics)

        /// <summary>
        /// À appeler par le système de clic (ClickInputSystem)
        /// pour permettre les spawns RandomOnClick.
        /// </summary>
        public void NotifyPlayerClick()
        {
            TrySpawnOnClick();
        }

        public bool HasActiveEvent()
        {
            return _activeEvent != null;
        }

        public EventDefinition GetActiveEvent()
        {
            return _activeEvent;
        }

        public float GetActiveEventRemainingTime()
        {
            return Mathf.Max(0f, _activeEventRemainingTime);
        }

        public void ResolveAndEndActiveEvent()
        {
            ResolveActiveEventReward();
            EndActiveEvent();
        }

        #endregion


        #region Main Methods (méthodes private)

        private void UpdateTimedEvents(float deltaTime)
        {
            _timerSinceLastTimedSpawn += deltaTime;

            foreach (EventDefinition def in m_Events)
            {
                if (!def || def.m_SpawnMode != EventSpawnMode.TimedInterval)
                {
                    continue;
                }

                if (_timerSinceLastTimedSpawn >= def.m_SpawnIntervalSeconds)
                {
                    TryActivateEvent(def);
                    _timerSinceLastTimedSpawn = 0f;
                    break;
                }
            }
        }

        private void UpdateActiveEvent(float deltaTime)
        {
            if (!_activeEvent)
            {
                return;
            }

            _activeEventRemainingTime -= deltaTime;
            if (_activeEventRemainingTime <= 0f)
            {
                EndActiveEvent();
            }
        }

        private void TrySpawnOnClick()
        {
            foreach (EventDefinition def in m_Events)
            {
                if (!def || def.m_SpawnMode != EventSpawnMode.RandomOnClick)
                {
                    continue;
                }

                float r = Random.value;
                if (r <= def.m_SpawnChanceOnClick)
                {
                    TryActivateEvent(def);
                    break;
                }
            }
        }

        private void TryActivateEvent(EventDefinition def)
        {
            if (!def)
            {
                return;
            }

            if (_activeEvent)
            {
                // Un event est déjà actif, on ignore pour garder le système simple.
                return;
            }

            _activeEvent = def;
            _activeEventRemainingTime = def.m_DurationSeconds;

            // Application du multiplicateur d'event
            double mult = def.m_ProductionMultiplier;
            if (mult <= 0d)
            {
                mult = 1d;
            }

            SetFact(CatsClickerFacts.m_eventProductionMultiplier, mult, FactDictionary.FactPersistence.Normal);

            if (m_LogEvents)
            {
                InfoInProgress($"Event '{def.m_EventId}' activé pour {def.m_DurationSeconds} sec (mult x{mult}).", this);
            }
        }

        private void EndActiveEvent()
        {
            if (!_activeEvent)
            {
                return;
            }

            // Reset multiplicateur
            SetFact(CatsClickerFacts.m_eventProductionMultiplier, 1d, FactDictionary.FactPersistence.Normal);

            if (m_LogEvents)
            {
                InfoDone($"Event '{_activeEvent.m_EventId}' terminé.", this);
            }

            _activeEvent = null;
            _activeEventRemainingTime = 0f;
        }

        private void ResolveActiveEventReward()
        {
            if (!_activeEvent)
            {
                return;
            }

            if (_activeEvent.m_RewardType == RewardType.None || _activeEvent.m_RewardValue <= 0d)
            {
                return;
            }

            double value = _activeEvent.m_RewardValue;

            switch (_activeEvent.m_RewardType)
            {
                case RewardType.Croquettes:
                    AddResource(CatsClickerFacts.m_croquettes, value);
                    break;

                case RewardType.Poissons:
                    AddResource(CatsClickerFacts.m_poissons, value);
                    break;

                case RewardType.Poulets:
                    AddResource(CatsClickerFacts.m_poulets, value, FactDictionary.FactPersistence.Persistent);
                    break;
            }

            if (m_LogEvents)
            {
                InfoDone($"Event reward: {_activeEvent.m_RewardType} +{value}", this);
            }
        }

        private void AddResource(string factKey, double value, FactDictionary.FactPersistence persistence = FactDictionary.FactPersistence.Normal)
        {
            double current = GetFact(factKey, 0d);
            current += value;
            SetFact(factKey, current, persistence);
        }
        
        #endregion


        #region Private and Protected

        private EventDefinition _activeEvent;
        private float _activeEventRemainingTime = 0f;
        private float _timerSinceLastTimedSpawn = 0f;

        #endregion
    }
}
