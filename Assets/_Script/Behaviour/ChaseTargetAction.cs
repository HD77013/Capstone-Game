using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTarget", story: "[Self] chases [Player] at [ChaseSpeed] at an offset of [ChaseOffset] while backing away at [BackOffset]", category: "Action", id: "b49cf284686f3d2212e4894a11bd6210")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> ChaseSpeed;
    [SerializeReference] public BlackboardVariable<float> ChaseOffset;
    [SerializeReference] public BlackboardVariable<float> BackOffset;

    [SerializeReference] public BlackboardVariable<bool> IsAttacking;
    [SerializeReference] public BlackboardVariable<bool> CanAttack;

    private Rigidbody2D _rb;

    protected override Status OnStart()
    {
        _rb = Self.Value.GetComponent<Rigidbody2D>();
        
        ChaseOffset.Value = Random.Range(-1.5f, 1.5f);
        BackOffset.Value = Random.Range(2.5f, 5.0f);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector2 playerPos = Player.Value.transform.position;
        Vector2 enemyPos = Self.Value.transform.position;
        
        float targetX = (playerPos.x + ChaseOffset.Value) - enemyPos.x;
        Vector2 follow = new Vector2(targetX, playerPos.y - enemyPos.y);
        
        float dir = Mathf.Sign(follow.x);
        
        float AwayDir = Mathf.Abs(playerPos.x - enemyPos.x);
        float awayDir = -Mathf.Sign(playerPos.x - enemyPos.x);

        bool isAttacking = IsAttacking.Value;
        bool canAttack = CanAttack.Value;

        if (AwayDir < BackOffset.Value && !isAttacking && !canAttack)
        {
     // Too close to the actual player — back away from them directly
            _rb.linearVelocity = new Vector2(ChaseSpeed.Value / 2 * awayDir, _rb.linearVelocity.y);
    //        animator.SetBool("Walking", true);
        }
        else if (AwayDir >= BackOffset.Value)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y);
    //        animator.SetBool("Walking", false);
        }
            
        if (isAttacking || canAttack)
        {
            // Move toward offset target position normally
           _rb.linearVelocity = new Vector2(ChaseSpeed.Value * dir, _rb.linearVelocity.y);
    //       animator.SetBool("Walking", true);
        }
        
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

