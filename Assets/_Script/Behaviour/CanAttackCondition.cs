using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanAttack", story: "If the last [LastAttackTime] >= the [cooldown] time, set [canAttack] to true", category: "Conditions", id: "0f457167f120e0cf82375ba9d0f2e694")]
public partial class CanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> Cooldown;
    [SerializeReference] public BlackboardVariable<float> LastAttackTime;

    public override bool IsTrue()
    {
        return Time.time - LastAttackTime.Value >= Cooldown.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
