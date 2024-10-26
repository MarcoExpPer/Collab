using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnemyBrain : MonoBehaviour
{
    [Header("Enemy Senses")]
    private IEnemySense[] _senses;

    public List<Sense_SO> sensesSO = new List<Sense_SO>();
    public bool showSensesGizmos = false;
    
    //First parameter is the sense result, second parameter the location sensed
    [HideInInspector] public UnityEvent<EEnemySenseResult, Vector3> enemySensesUpdated = new UnityEvent<EEnemySenseResult, Vector3>();
    private Coroutine _sensesCoroutine;
    public float sensesPeriod = 0.25f;

    [Header("Enemy States")] 
    [SerializeField] public E_IdleStateSO idleStateSO;
    [SerializeField] public E_AlertStateSO alertStateSO;
    [SerializeField] public E_InCombatStateSO inCombatStateSO;
    [SerializeField] public E_ReturnToIdleStateSO returnToIdleStateSO;
    [SerializeField] public E_HitStateSO hitStateSO;
    
    [SerializeField, HideInInspector] private E_IdleStateSO idleStateSOinstance;
    [SerializeField, HideInInspector] private E_AlertStateSO alertStateSOinstance;
    [SerializeField, HideInInspector] private E_InCombatStateSO inCombatStateSOinstance;
    [SerializeField, HideInInspector] private E_ReturnToIdleStateSO returnToIdleStateSOinstance;
    [SerializeField, HideInInspector] private E_HitStateSO hitStateSOinstance;
    
    public bool enableSmDebug = false;
    [SerializeField, HideInInspector] public UnityEvent<bool> onActivationTiggered = new UnityEvent<bool>();
    //STATES
    public StateMachineBase StateMachine;

    public E_IdleState IdleState;
    public E_AlertState AlertState;
    public E_InCombatComplexState InCombatComplexState;
    public E_ReturnToIdleState ReturnToIdleState;
    public E_HitState HitState;
    
    //CACHED COMPONENTS
    [SerializeField, HideInInspector] public NavMeshAgent navMeshAgent;
    [SerializeField, HideInInspector] private PlayerTrigger activateEnemyTrigger;
    
    //Work around to avoid useless warnings for creating GameObjects in on-validate
#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    private void _OnValidate()
    {   
        //With the delay call, this may be fired when this doesnt exist
        if (!this) return;
        
        enabled = false;
        
        //Create all necesary sense components in editor time
        _senses = new IEnemySense[sensesSO.Count];
        for (int i = 0; i < sensesSO.Count; ++i)
        {
            if (sensesSO[i])
            {
                _senses[i] = sensesSO[i].CreateSenseComponent(gameObject);
            }
        }
        
        activateEnemyTrigger = GetComponent<PlayerTrigger>();
        activateEnemyTrigger.enabled = false;

        if (idleStateSO)
        {
            idleStateSOinstance = Instantiate(idleStateSO);
            idleStateSOinstance.OnValidateSetup(this); 
        }

        if (alertStateSO)
        {
            alertStateSOinstance = Instantiate(alertStateSO);
            alertStateSOinstance.OnValidateSetup(this);
        }

        if (inCombatStateSO)
        {
            inCombatStateSOinstance = Instantiate(inCombatStateSO);
            inCombatStateSOinstance.OnValidateSetup(this);  
        }
        
        if (returnToIdleStateSO)
        {
            returnToIdleStateSOinstance = Instantiate(returnToIdleStateSO);
            returnToIdleStateSOinstance.OnValidateSetup(this);  
        }

        if (hitStateSO)
        {
            hitStateSOinstance = Instantiate(hitStateSO);
            hitStateSOinstance.OnValidateSetup(this);  
        }
    }
#endif
    private void OnPlayerEnterTrigger(GameObject playerGameObject)
    {
        enabled = true;
        onActivationTiggered.Invoke(true);
    }

    private void OnPlayerExitTrigger(GameObject playerGameObject)
    {
        enabled = false;
        
        foreach (var sense in _senses)
        {
            sense.SetActive(false);
        }
        
        onActivationTiggered.Invoke(false);
    }

    public void SetActiveSenses(bool activate)
    {
        if (_sensesCoroutine != null)
        {
            StopCoroutine(_sensesCoroutine);
        }

        if (activate)
        {
            foreach (var sense in _senses)
            {
                sense.SetActive(true);
            }
        }

        
        if (activate)
        {
            _sensesCoroutine = StartCoroutine(QuerySensesCoroutine());
        }
        
    }

    IEnumerator QuerySensesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(sensesPeriod);
            var queryResult = QuerySenses();
            enemySensesUpdated.Invoke(queryResult.Item1, queryResult.Item2);
        }
    }

    public (EEnemySenseResult, Vector3) QuerySenses()
    {
        EEnemySenseResult result = EEnemySenseResult.No;
        Vector3 targetLocation = Vector3.zero;
        
        foreach (var sense in _senses)
        {
            if (sense.GetSenseResult() == EEnemySenseResult.Unsure)
            {
                result = EEnemySenseResult.Unsure;
                targetLocation = sense.GetSenseLocation();
            }else if (sense.GetSenseResult() == EEnemySenseResult.Success)
            {
                result = EEnemySenseResult.Success;
                targetLocation = sense.GetSenseLocation();
                break;
            }
        }
        return (result, targetLocation);
    }

    public void Awake()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        
        StartCoroutine(DelayedStart());
    }

    public IEnumerator DelayedStart()
    {   
        //In editor, onValidate gets called before start. That creates some problem soooo the current workaround is to delay the start to allow onValidate to finish
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        
        if (!idleStateSOinstance)
        {
            idleStateSOinstance = Instantiate(idleStateSO);
            idleStateSOinstance.OnValidateSetup(this); 
        }

        if (!alertStateSOinstance)
        {
            alertStateSOinstance = Instantiate(alertStateSO);
            alertStateSOinstance.OnValidateSetup(this);
        }

        if (!inCombatStateSOinstance)
        {
            inCombatStateSOinstance = Instantiate(inCombatStateSO);
            inCombatStateSOinstance.OnValidateSetup(this);  
        }
        
        if (!returnToIdleStateSOinstance)
        {
            returnToIdleStateSOinstance = Instantiate(returnToIdleStateSO);
            returnToIdleStateSOinstance.OnValidateSetup(this);  
        }

        if (!hitStateSOinstance)
        {
            hitStateSOinstance = Instantiate(hitStateSO);
            hitStateSOinstance.OnValidateSetup(this);  
        }
        
        StateMachine = new StateMachineBase();
        StateMachine.enableDebug = enableSmDebug;
        
        IdleState = new E_IdleState(this, idleStateSOinstance, StateMachine);
        AlertState = new E_AlertState(this, alertStateSOinstance, StateMachine);
        InCombatComplexState = new E_InCombatComplexState(this, inCombatStateSOinstance, StateMachine);
        ReturnToIdleState = new E_ReturnToIdleState(this, returnToIdleStateSOinstance, StateMachine);
        HitState = new E_HitState(this, hitStateSOinstance, StateMachine);
        
        activateEnemyTrigger.onPlayerEnterTrigger.AddListener(OnPlayerEnterTrigger);
        activateEnemyTrigger.onPlayerExitTrigger.AddListener(OnPlayerExitTrigger);
        
        _senses = new IEnemySense[sensesSO.Count];
        for (int i = 0; i < sensesSO.Count; ++i)
        {
            if (sensesSO[i])
            {
                _senses[i] = sensesSO[i].CreateSenseComponent(gameObject);
            }
        }
        
        activateEnemyTrigger.enabled = true; //This trigger should always be enabled once the initialization of the enemy has finished
        navMeshAgent.enabled = true;
    }

    public void OnEnable()
    {
        StateMachine.Initialize(IdleState);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdate();
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }

    public void EnemyGotHit(bool isDead)
    {
        HitState.isDead = isDead;
        ((E_State)StateMachine.CurrentState).OnHit(isDead);
    }

    void OnDrawGizmosSelected()
    {
        if (showSensesGizmos)
        {
            foreach (var sense in _senses)
            {
                if (sense != null)
                {
                    sense.DrawDebugGizmos(gameObject);
                }
            }
        }
    }
}