using System.Collections.Generic;
using CatsClicker.Data;
using CatsClicker.Systems;
using TheFundation.Runtime;
using UnityEngine;

namespace CatsClicker.Runtime
{
    public class UiCatsPanel : FBehaviour
    {
        #region Publics

        [Header("Bindings")]
        public Transform m_ContentRoot;
        public UiCatEntry m_CatEntryPrefab;

        [Header("Data")]
        public CatSystem m_CatSystem;
        public List<CatDefinition> m_CatsDefinitions = new List<CatDefinition>();

        #endregion

        #region API Unity

        private void Start()
        {
            _GenerateEntries();
        }

        #endregion

        #region Utils (méthodes publics)

        // Rien de public pour l'instant

        #endregion

        #region Main Methods (méthodes private)

        private void _GenerateEntries()
        {
            if (m_ContentRoot == null || m_CatEntryPrefab == null || m_CatSystem == null)
            {
                Warning("UiCatsPanel not correctly configured.", this);
                return;
            }

            foreach (CatDefinition def in m_CatsDefinitions)
            {
                if (def == null)
                {
                    continue;
                }

                UiCatEntry instance = Instantiate(m_CatEntryPrefab, m_ContentRoot);
                instance.SetData(def, m_CatSystem);
            }
        }

        #endregion

        #region Private and Protected

        // Rien

        #endregion
    }
}
