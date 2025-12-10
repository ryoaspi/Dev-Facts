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
        
        public GoalChainDefinition m_tutorialChainDefinition;

        #endregion

        #region API Unity

        private void Update()
        {
            TickProduction(Time.deltaTime);
        }

        #endregion

        #region Utils (méthodes publics)

        public void Initialize()
        {
            InitializeInternal();
        }

        #endregion

        #region Main Methods (méthodes private)

        private void InitializeInternal()
        {
            Info("ResourceSystemCC initialized.");
        }

        private void TickProduction(float deltaTime)
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
            
            GoalsService.Notify("cc_collect_100", (int)croquettes);
            GoalsService.Notify("cc_collect_1000", (int)croquettes);
            GoalsService.Notify("cc_collect_10000", (int)croquettes);

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