using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "DetectPlayer", story: "Check if [Enemy] detects [Player] at [DetectionThrehold]", category: "Conditions", id: "8d8c9e28038d46a83f037c0f5c68f217")]
public partial class DetectPlayerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> DetectionThrehold;

    public override bool IsTrue()
    {
        if (Player.Value && Enemy.Value == null)
            return false;
        
        Vector2 playerPos = Player.Value.transform.position;
        float distance = Vector2.Distance(Enemy.Value.transform.position, playerPos);
        
        return distance < DetectionThrehold.Value;
    }

    public override void OnStart()
    {
        
    }

    public override void OnEnd()
    {
    }
}
