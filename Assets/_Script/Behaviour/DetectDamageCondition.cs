using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "DetectDamage", story: "[Self] is damaged via [Data]", category: "Conditions", id: "8a8f6afca358c9e013f811ca9f530ec6")]
public partial class DetectDamageCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> Data;

    public override bool IsTrue()
    {
        return Data.Value.isKnockedBack;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
