using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//inspired by samyam on Youtube: https://www.youtube.com/watch?v=ERAN5KBy2Gs&t=535s
public class InputManager : SingletonPattern<InputManager>
{
    public delegate void StartHold(Vector2 pos);
    public event StartHold StartHoldEvent;

    public delegate void PerformedHold(Vector2 pos);
    public event PerformedHold PerformedHoldEvent;

    public delegate void EndHold(Vector2 pos);
    public event EndHold EndHoldEvent;

    private Input input;


    private void Awake()
    {
        input = new Input();
    }

    void Start()
    {
        input.TouchControls.TouchHold.canceled += OnEndHold;
        input.TouchControls.TouchHold.performed += OnStartHold;
        input.TouchControls.TouchInput.performed += OnPerformedHold;

    }

    private void OnEnable()
    {
        input.TouchControls.Enable();
    }

    private void OnDisable()
    {
        input.TouchControls.Disable();
    }

    /// <summary>
    /// If anyone is subscribed to the OnStartHoldEvent, they will get the position
    /// of the touch
    /// </summary>
    /// <param name="ctx">The context of the input action</param>
    private void OnStartHold(InputAction.CallbackContext ctx)
    {
        StartHoldEvent?.Invoke(input.TouchControls.TouchPosition.ReadValue<Vector2>());
    }

    /// <summary>
    /// If anyone is subscribed to the OnEndHoldEvent, they will get the position
    /// of the touch    
    /// </summary>
    /// <param name="ctx">The context of the input action</param>
    private void OnEndHold(InputAction.CallbackContext ctx)
    {
        EndHoldEvent?.Invoke(input.TouchControls.TouchPosition.ReadValue<Vector2>());
    }

    /// <summary>
    /// If anyone is subscribed to the OnPerformedHoldEvent, they will get the position
    /// of the touch
    /// </summary>
    /// <param name="ctx">The context of the input action</param>
    private void OnPerformedHold(InputAction.CallbackContext ctx)
    {
        PerformedHoldEvent?.Invoke(input.TouchControls.TouchPosition.ReadValue<Vector2>());
    }
}
