using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTarget", story: "[Self] chases [Player] at [ChaseSpeed] at an offset of [ChaseOffset]", category: "Action", id: "b49cf284686f3d2212e4894a11bd6210")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> ChaseSpeed;
    [SerializeReference] public BlackboardVariable<float> ChaseOffset;

    private Rigidbody2D _rb;

    protected override Status OnStart()
    {
        _rb = Self.Value.GetComponent<Rigidbody2D>();
        
        ChaseOffset.Value = Random.Range(-1.5f, 1.5f);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector2 playerPos = Player.Value.transform.position;
        Vector2 enemyPos = Self.Value.transform.position;
        
        float targetX = (playerPos.x + ChaseOffset.Value) - enemyPos.x;
        Vector2 follow = new Vector2(targetX, playerPos.y - enemyPos.y);
        
        float dir = Mathf.Sign(follow.x);
        
        _rb.linearVelocity = new Vector2(ChaseSpeed.Value * dir, _rb.linearVelocity.y);
    //       animator.SetBool("Walking", true);
          
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

