using UnityEngine;

namespace TheFundation.Runtime
{
    public static class GameHealthCheck
    {
        public static GameHealthReport RunFullCheck()
        {
            var report = new GameHealthReport();

            CheckVersion(report);
            CheckLocalization(report);
            CheckFacts(report);
            CheckGoals(report);
            CheckGoalChains(report);
            CheckPlatform(report);

            Debug.Log($"[GameHealth] Terminé. Errors: {report.m_errors.Count}, Warnings: {report.m_warnings.Count}");
            return report;
        }


        #region CHECK VERSION

        private static void CheckVersion(GameHealthReport report)
        {
            // Vérifier que VersionDefinition est assigné
            if (VersionService.m_definition == null)
            {
                report.AddWarning("VersionService.m_definition est NULL (aucune version interne définie).");
                return;
            }

            string fwVersion = VersionService.FrameworkVersion;

            if (fwVersion == "0.0.0")
                report.AddWarning("FrameworkVersion = 0.0.0 (peut-être non configuré).");

            // Vérifier le Fact stocké
            if (GameManager.Facts.TryGetFact("game_version", out string savedVersion))
            {
                if (savedVersion != fwVersion)
                    report.AddWarning($"Version mismatch. Save={savedVersion}, Framework={fwVersion}");
            }
        }

        #endregion


        #region CHECK LOCALIZATION

        private static void CheckLocalization(GameHealthReport report)
        {
            if (LocalizationManager.Instance == null)
            {
                report.AddError("LocalizationManager.Instance est NULL.");
                return;
            }

            if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
                report.AddError("LocalizationManager.CurrentLanguage est vide.");

            // Vérifie juste la présence du fichier minimum
            TextAsset file = Resources.Load<TextAsset>($"Localization/{LocalizationManager.CurrentLanguage}");

            if (!file)
                report.AddWarning($"Aucun fichier de langue trouvé pour '{LocalizationManager.CurrentLanguage}'.");
        }

        #endregion


        #region CHECK FACTS

        private static void CheckFacts(GameHealthReport report)
        {
            // Basic consistency check : vérifier absence de clé vide
            int problemCount = 0;

            // On utilise GameManager.Facts.GetAllKeys() SI tu l’as
            // Sinon on ne peut pas parcourir
            var allKeys = GameManager.Facts.GetAllKeys();

            foreach (var key in allKeys)
            {
                if (string.IsNullOrEmpty(key))
                {
                    problemCount++;
                    report.AddError("Fact avec clé vide détectée.");
                }
            }

            if (problemCount == 0)
                Debug.Log("[GameHealth] Facts OK.");
        }

        #endregion


        #region CHECK GOALS

        private static void CheckGoals(GameHealthReport report)
        {
            if (GoalsService.Collection == null)
            {
                report.AddWarning("GoalsService.Collection n'est pas assigné.");
                return;
            }

            foreach (var goal in GoalsService.Collection.m_Goals)
            {
                if (goal == null)
                {
                    report.AddError("GoalDefinition NULL dans GoalsCollection.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(goal.m_Key))
                    report.AddError("GoalDefinition avec m_Key vide.");

                if (goal.m_TargetValue < 1)
                    report.AddWarning($"Goal '{goal.m_Key}' a un TargetValue < 1 (⚠ incohérent si pas instantané).");

                if (goal.m_IsInstantGoal && goal.m_TargetValue != 1)
                    report.AddWarning($"Goal '{goal.m_Key}' est instantané mais m_TargetValue != 1.");
            }
        }

        #endregion


        #region CHECK GOAL CHAINS

        private static void CheckGoalChains(GameHealthReport report)
        {
            // On détecte toutes les chaines dans Resources
            var chains = Resources.LoadAll<GoalChainDefinition>("");

            foreach (var chain in chains)
            {
                if (chain == null) continue;

                if (string.IsNullOrWhiteSpace(chain.m_ChainKey))
                    report.AddError("GoalChain sans m_ChainKey.");

                if (chain.m_steps == null || chain.m_steps.Length == 0)
                    report.AddError($"GoalChain '{chain.m_ChainKey}' sans étapes.");

                foreach (var step in chain.m_steps)
                {
                    if (step == null)
                        report.AddError($"GoalChain '{chain.m_ChainKey}' contient une étape NULL.");
                }
            }
        }

        #endregion


        #region CHECK PLATFORM

        private static void CheckPlatform(GameHealthReport report)
        {
            string family = GameManager.Facts.GetFact("platform_family", "Unknown");

            if (family == "Unknown")
                report.AddWarning("PlatformizerService n'a pas défini 'platform_family'.");
        }

        #endregion
    }
}
