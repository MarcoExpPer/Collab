using System;
using UnityEngine;
using UnityEngine.InputSystem;

using static InputActions;

public class AlwaysActiveInput : MonoBehaviour
{
    public AlwaysActiveActions AlwaysActiveActions { get; private set; }
    
    void Awake()
    {
        AlwaysActiveActions = new InputActions().AlwaysActive;
        AlwaysActiveActions.Enable();
    }
}
