using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    [CreateAssetMenu(
        fileName = "EventDefinition",
        menuName = "CatsClicker/Event Definition")]
    public class EventDefinition : ScriptableObject
    {
        #region Publics

        [Header("Identification")]
        public string m_EventId;
        public string m_Label;
        public string m_Description;

        [Header("Spawn")]
        public EventSpawnMode m_SpawnMode = EventSpawnMode.RandomOnClick;
        [Tooltip("Probabilité [0..1] de spawn sur un clic joueur (pour RandomOnClick).")]
        [Range(0f, 1f)]
        public float m_SpawnChanceOnClick = 0.01f;
        [Tooltip("Intervalle en secondes entre deux tentatives de spawn (pour TimedInterval).")]
        public float m_SpawnIntervalSeconds = 60f;

        [Header("Durée")]
        [Tooltip("Durée de l'événement en secondes.")]
        public float m_DurationSeconds = 10f;

        [Header("Récompense")]
        public RewardType m_RewardType = RewardType.Croquettes;
        public double m_RewardValue = 0d;

        [Header("Multiplicateur de production pendant l'événement")]
        [Tooltip("Ex: 2.0 = production x2 pendant l'event.")]
        public double m_ProductionMultiplier = 1d;

        #endregion
    }
}