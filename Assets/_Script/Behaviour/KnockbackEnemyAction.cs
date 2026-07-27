using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "KnockbackEnemy", story: "[Self] is knocked away by a force of [Float]", category: "Action", id: "421fbb44f3f970a18e2929e76da1484b")]
public partial class KnockbackEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Float;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> data;

    private Rigidbody2D rb;

    protected override Status OnStart()
    {
        if (rb == null) {
            rb = Self.Value.GetComponent<Rigidbody2D>();
        }
        
        Vector2 directionToTarget = ((Vector2)Self.Value.transform.position - (Vector2)data.Value.target.position).normalized;
        Vector2 knockbackDir = new Vector2(directionToTarget.x, 0f).normalized;
        
        rb.linearVelocity = Vector2.zero; // Cancel existing movement
        rb.linearVelocity = new Vector2(knockbackDir.x * data.Value.knockbackForce, 0f);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

