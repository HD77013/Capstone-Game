using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IfPlayerBlocking", story: "[Self] reads if [Data] is attacking", category: "Conditions", id: "973537d7e8ebeb2d2166e3916404c9dd")]
public partial class IfPlayerBlockingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> Data;

    public override bool IsTrue()
    {
        return Data.Value.isBlocking;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
