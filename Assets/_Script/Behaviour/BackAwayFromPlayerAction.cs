using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BackAwayFromPlayer", story: "[Self] distances from [Player] at [Distance] at a speed of [Speed]", category: "Action", id: "5843a3a9bb1dfe227a6fe55049e00f31")]
public partial class BackAwayFromPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<float> Speed;
    private float _awayDir;
    private Rigidbody2D _rb;

    protected override Status OnStart()
    {
        _rb = Self.Value.GetComponent<Rigidbody2D>();
        
        Distance.Value = Random.Range(2.5f, 5.0f);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _awayDir = Mathf.Abs(Player.Value.transform.position.x - Self.Value.transform.position.x);
        float awayDir = -Mathf.Sign(Player.Value.transform.position.x - Self.Value.transform.position.x);
        
        if (_awayDir < Distance.Value)
        {
            // Too close to the actual player — back away from them directly
            _rb.linearVelocity = new Vector2(Speed.Value * awayDir, _rb.linearVelocity.y);
        }
        else
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

