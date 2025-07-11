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




        void Update()
        {
        
        }
        
        #endregion
        
        
        #region Utils

        public void OnLoadSlot(int slot)
        {
            GameManager.LoadGameFromSlot(slot);
            
        }

        public void OnSaveSlot(int slot)
        {
            GameManager.SaveGameToSlot(slot);
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
