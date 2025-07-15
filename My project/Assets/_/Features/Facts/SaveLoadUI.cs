using UnityEngine;

namespace TheFundation.Runtime
{
    public class SaveLoadUI : MonoBehaviour
    {
        #region Api Unity
        void Start()
        {
            RefreshSlotsUI();
        }
        
        #endregion
        
        
        #region Utils

        public void OnLoadSlot(int slot)
        {
            GameManager.LoadGameFromSlot(slot);
            
            // // Lecture des Facts
            // if (GameManager.m_gameFacts.TryGetFact("playerName", out string name) &&
            //     GameManager.m_gameFacts.TryGetFact("gold", out int gold) &&
            //     GameManager.m_gameFacts.TryGetFact("Race", out string race))
            // {
            //     Debug.Log($"[Slot {slot}] Nom : {name}, Gold : {gold}, Race : {race}");
            // }
            //
            // else
            // {
            //     Debug.LogWarning($"[Slot {slot}] Données incomplètes ou absentes après chargement. ");
            // }
            
        }

        public void OnSaveSlot(int slot)
        {
            GameManager.SaveGameToSlot(slot);
        }
        
        public void OnDeleteSlot(int slot)
        {
            GameManager.DeleteSaveSlot(slot);
        }
        
        #endregion
        
        
        #region Main Methods
        
        private void RefreshSlotsUI()
        {
            foreach (Transform child in _slotsContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _maxSlots; i++)
            {
                GameObject slotGO = Instantiate(_slotButtonPrefab, _slotsContainer);
                slotGO.name = $"Slot {i}";
                
                SlotButtonUI slotUI = slotGO.GetComponent<SlotButtonUI>();
                slotUI.Setup(i, this);
            }
        }
        
        #endregion
        
        
        #region Private And Protected

        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private GameObject _slotButtonPrefab;
        [SerializeField] private int _maxSlots = 10;

        #endregion
    }
}
