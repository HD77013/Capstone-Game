using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EmitBloodOnEnemy", story: "[Self] emits [BloodPartice] on itself", category: "Action", id: "ea9c1adbfaba43c754fb1de1d5b04b66")]
public partial class EmitBloodOnEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<ParticleSystem> BloodPartice;

    protected override Status OnStart()
    {
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

