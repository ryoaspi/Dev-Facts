using System;
using CatsClicker.Runtime;
using TheFundation.Runtime;

namespace CatsClicker.Systems
{
    public class PrestigeSystem : FBehaviour
    {
        #region Publics

        public event Action m_OnPrestigeApplied;

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

            InfoDone($"Prestige applied. +{pouletsToGain} poulets (total: {poulets}).");

            m_OnPrestigeApplied?.Invoke();
        }

        #endregion

        #region Private and Protected

        // Rien pour l’instant

        #endregion
    }
}
