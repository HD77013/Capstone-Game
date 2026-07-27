using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckIfDamaged", story: "Check if [Self] is assumed damaged by [Script]", category: "Conditions", id: "f1b149eb3619eff5a67dabb1dec82b5d")]
public partial class CheckIfDamagedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> Script;

    public override bool IsTrue()
    {
        return Script.Value.isDamaged;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
