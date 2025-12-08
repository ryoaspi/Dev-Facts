namespace CatsClicker.Runtime
{
    public class CatsClickerFacts
    {
        #region Publics

        // Ressources principales
        public const string m_croquettes = "cc_croquettes";
        public const string m_poissons   = "cc_poissons";
        public const string m_poulets    = "cc_poulets";

        // Production totale (par seconde)
        public const string m_totalProductionPerSecond = "cc_total_prod_per_sec";

        // First launch
        public const string m_hasLaucheBefore = "cc_has_lauche_before";

        // Multiplicateurs globaux (upgrades)
        public const string m_multGlobalProduction = "cc_mult_global_production";
        public const string m_multClick           = "cc_mult_click";

        public static string GetCatLevelKey(string catID)
        {
            return $"cc_cat_{catID}_level";
        }

        public static string GetUpgradeLevelKey(string upgradeId)
        {
            return $"cc_upgrade_{upgradeId}_level";
        }

        #endregion
    }
}