using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomPatrolPoint", story: "[Self] patrols along [RightPos] to [LeftPos]", category: "Action", id: "d2143228c4693e7b85d9e839e22dd17a")]
public partial class RandomPatrolPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> RightPos;
    [SerializeReference] public BlackboardVariable<float> LeftPos;
    [SerializeReference] public BlackboardVariable<float> WalkSpeed;
    [SerializeReference] public BlackboardVariable<float> MinWalkTime, MaxWalkTime;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> EnemyData;
    [SerializeReference] public BlackboardVariable<bool> wandering;
    
    private Vector2 patrolCenter;

    private Rigidbody2D _rb;

    private float _randomTime, _timer;
    
    protected override Status OnStart()
    {
        wandering.Value = true;
        
        patrolCenter = Self.Value.transform.position;
        
        _rb = Self.Value.GetComponent<Rigidbody2D>();
        
        _randomTime = Random.Range(MinWalkTime, MaxWalkTime);
        
        _timer = Random.Range(0f, _randomTime);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _timer += Time.deltaTime;
            
        if (Self.Value.transform.position.x >= RightPos.Value + patrolCenter.x || 
            Self.Value.transform.position.x <= LeftPos.Value + patrolCenter.x)
        {
            wandering.Value = false;
            EnemyData.Value.facingDirection *= -1;
            
            _timer = 0;
            _randomTime = Random.Range(MinWalkTime.Value, MaxWalkTime.Value);
        }

        if (_randomTime <= _timer)
        {
            EnemyData.Value.facingDirection *= -1;
            wandering.Value = false;
        }
        
        if (wandering.Value)
            _rb.linearVelocity = new Vector2(WalkSpeed.Value * EnemyData.Value.facingDirection, _rb.linearVelocity.y);
        else
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        
        
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}