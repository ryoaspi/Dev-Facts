using CatsClicker.Runtime;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Systems
{
    public class ResourceSystem : FBehaviour
    {
        #region Publics

        [Header("Debug")]
        public bool m_LogEachTick = false;

        #endregion

        #region API Unity

        private void Update()
        {
            _TickProduction(Time.deltaTime);
        }

        #endregion

        #region Utils (méthodes publics)

        public void Initialize()
        {
            _InitializeInternal();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void _InitializeInternal()
        {
            Info("ResourceSystemCC initialized.");
        }

        private void _TickProduction(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            double productionPerSecond = GetFact(CatsClickerFacts.m_totalProductionPerSecond, 0d);
            if (productionPerSecond <= 0d)
            {
                return;
            }

            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);
            double produced = productionPerSecond * deltaTime;

            croquettes += produced;

            SetFact(CatsClickerFacts.m_croquettes, croquettes, FactDictionary.FactPersistence.Normal);

            if (m_LogEachTick)
            {
                Info($"Tick: +{produced:F2} croquettes (total: {croquettes:F2})");
            }
        }

        #endregion

        #region Private and Protected

        // Pas de champs privés pour l’instant

        #endregion
    }
}