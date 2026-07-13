using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangeState", story: "Set [EnemyStates] to [State]", category: "Action", id: "0bf2d13eadeff67e8d2b385e9ec5df28")]
public partial class ChangeStateAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyStates> EnemyStates;
    [SerializeReference] public BlackboardVariable<EnemyStates> State;
    protected override Status OnStart()
    {
        EnemyStates.Value = State.Value;
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

