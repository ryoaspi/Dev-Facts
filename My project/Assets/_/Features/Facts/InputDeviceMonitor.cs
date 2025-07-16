using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFundation.Runtime
{
    public class InputDeviceMonitor : MonoBehaviour
    {
        
        #region Api Unity

        private void OnEnable()
        {
            _isActive = true;
            _anyButtonPress = new InputAction(type : InputActionType.PassThrough, binding : "<Keyboard>/anyKey");
            _anyButtonPress.AddBinding("<Gamepad>/*"); // Attrape tout sur manette
            _anyButtonPress.AddBinding("<Mouse>/*"); // Attrape tout sur souris

            _anyButtonPress.performed += OnAnyInput;
            _anyButtonPress.Enable();
        }

        private void OnDisable()
        {
            _isActive = false;

            if (_anyButtonPress != null)
            {
                _anyButtonPress.performed -= OnAnyInput;
                _anyButtonPress.Disable();
                _anyButtonPress.Dispose();
            }
        }

        private void OnAnyInput(InputAction.CallbackContext control)
        {
            if (!_isActive) return;
            var device = control.control.device;
            
            if (device == _lastDevice) return;
            _lastDevice = device;
            
            InputPlatformDetector.DetectInputScheme(device);
            GameManager.m_gameFacts.SetFact("inputScheme", InputPlatformDetector.CurrentScheme.ToString(), FactDictionary.FactPersistence.Normal);
            
            // Rafraîchit les icônes/textes
            InputIconEvents.InvokeOnInputSchemeChanged();
        }

        #endregion
        
        
        #region Utils


        
        #endregion
        
        
        #region Private And Protected

        private InputAction _anyButtonPress;
        private InputDevice _lastDevice;
        private bool _isActive = false;

        #endregion
    }
}
