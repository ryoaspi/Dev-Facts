using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "InputManager")]
public class NewScriptableObjectScript : ScriptableObject
{

    #region Api Unity

    private void OnEnable()
    {
        if (_PlayerInput == null)
        {
            _PlayerInput = new PlayerInput();
        }
    }

    #endregion
    
    
    #region Utils

    public PlayerInput GetPlayerInput()
    {
        return _PlayerInput;
    }
    
    #endregion
    
    
    #region Privata And Protected
    
    private PlayerInput _PlayerInput;
    
    #endregion
}
