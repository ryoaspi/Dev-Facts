using System.Collections.Generic;
using CatsClicker.Data;
using CatsClicker.Systems;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    public class UiUpgradePanel : FBehaviour
    {
        #region Publics

        [Header("Bindings")]
        public Transform m_ContentRoot;
        public UiUpgradeEntry m_UpgradeEntryPrefab;

        [Header("Data")]
        public UpgradeSystem m_UpgradeSystem;
        public List<UpgradeDefinition> m_UpgradeDefinitions = new List<UpgradeDefinition>();

        #endregion

        #region API Unity

        private void Start()
        {
            _GenerateEntries();
        }

        #endregion

        #region Utils (méthodes publics)

        // Rien

        #endregion

        #region Main Methods (méthodes private)

        private void _GenerateEntries()
        {
            if (m_ContentRoot == null || m_UpgradeEntryPrefab == null || m_UpgradeSystem == null)
            {
                Warning("UiUpgradePanel not correctly configured.", this);
                return;
            }

            foreach (UpgradeDefinition def in m_UpgradeDefinitions)
            {
                if (def == null)
                {
                    continue;
                }

                UiUpgradeEntry instance = Instantiate(m_UpgradeEntryPrefab, m_ContentRoot);
                instance.SetData(def, m_UpgradeSystem);
            }
        }

        #endregion

        #region Private and Protected

        // Rien

        #endregion
    }
}