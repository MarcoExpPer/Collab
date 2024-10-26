
using UnityEngine;
using VInspector;

[CreateAssetMenu(menuName = "Enemies/Alert/StaticAlert")]
public class E_AlertStateSO : E_StateSO
{
    [SerializeField] public AlertSprite alertSpritePrefab;
    public Vector3 spriteOffset;
    public bool lookAtSense = true;
    [ShowIf("lookAtSense")] 
    public float lookAtRotationSpeed = 2f;
        
    [SerializeField] private AlertSprite AlertSprite;

    public float timeUnsureToInCombat = 5f;
    [Tooltip("If this value is higher than 0, the enemy will mantain the alert state for a few seconds after losing sense of player")]
    public float mantainAlertAfterSenseLost = 5;
    
    private float _timeOfLastSense;
    public float currentTimer = 0f;
    
    public override void Initialize(EnemyBrain inBrain)
    {
        base.Initialize(inBrain);
        
        AlertSprite = Instantiate(alertSpritePrefab, inBrain.transform);
        AlertSprite.transform.localPosition = Vector3.zero + spriteOffset;
        AlertSprite.Setup();

        _timeOfLastSense = 0;
    }

    public override void Enter()
    {
        base.Enter();
        AlertSprite.enabled = true;
        
        //Number between 0 and timeUnsureToInCombat (4)
        float timeSinceLastSense = Mathf.Min(timeUnsureToInCombat, Time.time - _timeOfLastSense);
        //We want currentTimer to be reduced by this amount, so the enemy has some sort of "memory"
        currentTimer = Mathf.Max(0, currentTimer - timeSinceLastSense);
    }

    public override void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= timeUnsureToInCombat)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.InCombatComplexState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        AlertSprite.enabled = false;

        _timeOfLastSense = Time.time;
    }
}
