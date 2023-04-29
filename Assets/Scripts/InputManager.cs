using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private ActionMap _actionMap;

    public event EventHandler OnUpAction;
    public event EventHandler OnLeftAction;
    public event EventHandler OnRightAction;
    public event EventHandler OnDownAction;
    
    public event EventHandler OnPauseAction;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _actionMap = new ActionMap();
        _actionMap.Player.Enable();
        
        _actionMap.Player.Up.performed += OnUpPerformed;
        _actionMap.Player.Left.performed += OnLeftPerformed;
        _actionMap.Player.Rigth.performed += OnRightPerformed;
        _actionMap.Player.Down.performed += OnDownPerformed;

        _actionMap.Player.Pause.performed += OnPausePerformed;

    }

    private void OnUpPerformed(InputAction.CallbackContext obj)
    {
        OnUpAction?.Invoke(this,EventArgs.Empty);
    }
    
    private void OnLeftPerformed(InputAction.CallbackContext obj)
    {
        OnLeftAction?.Invoke(this,EventArgs.Empty);
    }
    
    private void OnRightPerformed(InputAction.CallbackContext obj)
    {
        OnRightAction?.Invoke(this,EventArgs.Empty);
    }
    
    private void OnDownPerformed(InputAction.CallbackContext obj)
    {
        OnDownAction?.Invoke(this,EventArgs.Empty);
    }
    
    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }


    private void OnDestroy()
    {
        _actionMap.Player.Up.performed -= OnUpPerformed;
        _actionMap.Player.Left.performed -= OnLeftPerformed;
        _actionMap.Player.Rigth.performed -= OnRightPerformed;
        _actionMap.Player.Down.performed -= OnDownPerformed;

        _actionMap.Player.Pause.performed -= OnPausePerformed;
    }
}