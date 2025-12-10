using System;
using CatsClicker.Runtime;
using TheFundation.Runtime;

namespace CatsClicker.Systems
{
    public class PrestigeSystem : FBehaviour
    {
        #region Publics

        public event Action m_OnPrestigeApplied;
        public GoalChainDefinition m_tutorialChainDefinition;

        #endregion

        #region API Unity

        // Rien de spécial pour l’instant

        #endregion

        #region Utils (méthodes publics)

        public void Initialize()
        {
            Info("PrestigeSystemCC initialized.");
        }

        public bool CanPrestige(out int pouletsToGain)
        {
            return _CanPrestigeInternal(out pouletsToGain);
        }

        public void ApplyPrestige()
        {
            _ApplyPrestigeInternal();
        }

        #endregion

        #region Main Methods (méthodes private)

        private bool _CanPrestigeInternal(out int pouletsToGain)
        {
            double croquettes = GetFact(CatsClickerFacts.m_croquettes, 0d);
            double poissons = GetFact(CatsClickerFacts.m_poissons, 0d);

            double score = croquettes + (poissons * 10d);
            double raw = Math.Sqrt(score / 1_000d);

            pouletsToGain = (int)Math.Floor(raw);

            return pouletsToGain > 0;
        }

        private void _ApplyPrestigeInternal()
        {
            if (!CanPrestige(out int pouletsToGain))
            {
                Warning("Cannot prestige: not enough score.");
                return;
            }

            double poulets = GetFact(CatsClickerFacts.m_poulets, 0d);
            poulets += pouletsToGain;

            SetFact(CatsClickerFacts.m_poulets, poulets, FactDictionary.FactPersistence.Persistent);

            SetFact(CatsClickerFacts.m_croquettes, 0d, FactDictionary.FactPersistence.Normal);
            SetFact(CatsClickerFacts.m_poissons, 0d, FactDictionary.FactPersistence.Normal);
            SetFact(CatsClickerFacts.m_totalProductionPerSecond, 0d, FactDictionary.FactPersistence.Normal);
            
            // OBJECTIF : premier prestige
            GoalChainService.NotifyChain("cc_tutorial_chain", m_tutorialChainDefinition, "cc_first_prestige", 1);
            GoalsService.Notify("cc_first_prestige", 1);

            // OBJECTIF : prestige total
            int count = GetFact("cc_prestige_total", 0) + 1;
            SetFact("cc_prestige_total", count, FactDictionary.FactPersistence.Persistent);
            GoalsService.Notify("cc_prestige5", count);

            // OBJECTIF : total poulets cumulés
            GoalsService.Notify("cc_poulets10", (int)poulets);


            InfoDone($"Prestige applied. +{pouletsToGain} poulets (total: {poulets}).");

            m_OnPrestigeApplied?.Invoke();
        }

        #endregion

        #region Private and Protected

        // Rien pour l’instant

        #endregion
    }
}
