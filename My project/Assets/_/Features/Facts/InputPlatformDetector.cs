using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFundation.Runtime
{
    public class InputPlatformDetector
    {
        public enum InputScheme
        {
            KeyboardMouse,
            XboxController,
            PlayStationController,
            GenericController
        }
        
        public static InputScheme CurrentScheme { get; private set; }

        public static void DetectInputScheme(InputDevice device)
        {
            string name = device.name.ToLower();
            string displayName = device.displayName.ToLower();

            if (device is Gamepad)
            {
                if ( name.Contains("xbox") || displayName.Contains("xbox"))
                    CurrentScheme = InputScheme.XboxController;
                else if ( name.Contains("dualshock") || displayName.Contains("playstation"))
                    CurrentScheme = InputScheme.PlayStationController;
                else
                    CurrentScheme = InputScheme.GenericController;    
            }
            
            else if (device is Keyboard || device is Mouse)
            {
                CurrentScheme = InputScheme.KeyboardMouse;
            }
            else
            {
                CurrentScheme = InputScheme.GenericController;
            }
        }
    }
}
