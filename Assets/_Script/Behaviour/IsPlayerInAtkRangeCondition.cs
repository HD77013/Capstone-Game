using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsPlayerInAtkRange", story: "If [EnemyScript] detects [Player]", category: "Conditions", id: "71bb6869092f89d46a8fea35809ae86d")]
public partial class IsPlayerInAtkRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyScript> EnemyScript;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    public override bool IsTrue()
    {
        return EnemyScript.Value.CheckAttack();
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
