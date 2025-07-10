using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFundation.Runtime
{
    public class SlotButtonUI : MonoBehaviour
    {
        #region Utils

        public void Setup(int slotIndex, SaveLoadUI parentUI)
        {
            _slotIndex = slotIndex;
            _parentUI = parentUI;

            UpdateLabel();
            
            _loadButton.onClick.AddListener(() => OnLoad());
            _saveButton.onClick.AddListener(() => OnSave());
            _deleteButton.onClick.AddListener(()=> OnDelete());
        }
        
        #endregion
        
        
        #region Main Methods

        private void OnLoad()
        {
            GameManager.LoadGameFromSlot(_slotIndex);
        }

        private void OnSave()
        {
            GameManager.SaveGameToSlot(_slotIndex);
            UpdateLabel();
        }

        private void OnDelete()
        {
            GameManager.DeleteSaveSlot(_slotIndex);
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            bool exists = GameManager.HasSaveInSlot(_slotIndex);
            _slotLabel.text = $"slot {_slotIndex} : {(exists ? "Existant" : "Vide")}";
            
            _loadButton.interactable = exists;
            _deleteButton.interactable = exists;
        }
        
        #endregion
        
        
        #region Private And Protected

        [SerializeField] private TMP_Text _slotLabel;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _deleteButton;

        private int _slotIndex;
        private SaveLoadUI _parentUI;

        #endregion
    }
}
