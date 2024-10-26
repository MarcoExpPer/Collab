using UnityEngine;


public class InputManager : MonoBehaviour
{
    [HideInInspector] public GroundedInput groundedInput;
    [HideInInspector] public UsingItemInput usingItemInput;
    [HideInInspector] public SwimmingInput swimmingInput;
    [HideInInspector] public FlyingInput flyingInput;
    [HideInInspector] public OnlyUIInput onlyUIInput;
    [HideInInspector] public QuickInventoryInput quickInventoryInput;
    [HideInInspector] public AlwaysActiveInput alwaysActiveInput;
    
    public void Awake()
    {
        groundedInput = GetComponent<GroundedInput>();
        usingItemInput = GetComponent<UsingItemInput>();
        swimmingInput = GetComponent<SwimmingInput>();
        flyingInput = GetComponent<FlyingInput>();
        onlyUIInput = GetComponent<OnlyUIInput>();
        quickInventoryInput = GetComponent<QuickInventoryInput>();
        alwaysActiveInput = GetComponent<AlwaysActiveInput>();
    }
}
