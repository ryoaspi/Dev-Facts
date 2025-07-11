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
            
            // string slotText = LocalizationManager.m_Instance.GetText("slot");
            string existsText = LocalizationManager.m_Instance.GetText(exists ? "exists" : "empty");
            
            var localized = _slotLabel.GetComponent<LocalizedText>();
            if (localized != null)
            {
                localized.SetKey("slot_exists");
                localized.SetFormattedText(_slotIndex +1 , existsText);
            }
            else
            {
                _slotLabel.text = $"Slot {_slotIndex + 1} : {existsText}";
            }
            
            _loadButton.GetComponentInChildren<TMP_Text>().text = LocalizationManager.m_Instance.GetText("load");
            _saveButton.GetComponentInChildren<TMP_Text>().text = LocalizationManager.m_Instance.GetText("save");
            _deleteButton.GetComponentInChildren<TMP_Text>().text = LocalizationManager.m_Instance.GetText("delete");
            
            _loadButton.interactable = exists;
            _deleteButton.interactable = exists;
            
            Debug.Log($"Slot {_slotIndex +1 } : {exists}");
            
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
