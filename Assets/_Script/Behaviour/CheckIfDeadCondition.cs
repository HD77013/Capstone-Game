using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckIfDead", story: "Check if [Self] is assumed dead by [Script]", category: "Conditions", id: "98d96eda67e9b30f5528b794705bb5a3")]
public partial class CheckIfDeadCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> Script;

    public override bool IsTrue()
    {
        return Script.Value.isDead;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
