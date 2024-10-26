
using System;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class PlayerSMController : MonoBehaviour
{
    //Animation Parameters
    public static string IsMovingParam = "IsMoving";
    public static string SwordSlashTrigger = "SwordSlash";
    public static string IsDraggingParam = "IsDragging";
    
    public StateMachineBase StateMachine { get; private set; }
    
    public PL_GroundState PlGroundState { get; private set; }
    public PL_UserInterfaceState PlUserInterfaceState { get; private set; }
    public PL_DebugState PlDebugState { get; private set; }
    
    //Un used
    public PL_SwimmingState PlSwimmingState { get; private set; }
    
    
    private void Awake()
    {
        GameManager.Instance.playerSmController = this;
    }
    private void Start()
    {
        StateMachine = new StateMachineBase();
        
        PlGroundState = new PL_GroundState(this, StateMachine);
        PlUserInterfaceState = new PL_UserInterfaceState(this, StateMachine);
        PlDebugState = new PL_DebugState(this, StateMachine);
            
        PlSwimmingState = new PL_SwimmingState(gameObject, StateMachine);
        

        StartCoroutine(StartSM());
        enabled = false;
    }
    //This fixes the weird behaviour of player still doing running animations on the first frame after being teleported
    public IEnumerator StartSM()
    {
        yield return new WaitForNextFrameUnit();
        StateMachine.Initialize(PlGroundState);
        enabled = true;
    }
    
    public void Update()
    {
        StateMachine.Update();
    }

    public void OnDestroy()
    {
        StateMachine.CurrentState.Exit();
    }
}
