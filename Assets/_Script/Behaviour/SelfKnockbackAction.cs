using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfKnockback", story: "[Self] will be knockbacked. Potentially from a source", category: "Action", id: "d62c93bedac1b21737df3f82cd8a8652")]
public partial class SelfKnockbackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> Data;

    private Rigidbody2D rb;

    protected override Status OnStart()
    {
        rb = Self.Value.GetComponent<Rigidbody2D>();

        if (Data.Value.attacker != null)
        {
            Debug.Log("Knockback");
            
            Vector2 directionToTarget = ((Vector2)Self.Value.transform.position - (Vector2)Data.Value.attacker.position)
                .normalized;
            Vector2 knockbackDir = new Vector2(directionToTarget.x, 0f).normalized;

            rb.linearVelocity = Vector2.zero; // Cancel existing movement
            rb.linearVelocity = new Vector2(knockbackDir.x * Data.Value.force, 0f);
        }
        else
        {
            Debug.Log("Source is missing!");    
            return Status.Failure;
        }

        
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

