using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyBlock", story: "[Self] initiates block", category: "Action", id: "cbfb3b2afd3dd7ed0749227fb604003a")]
public partial class EnemyBlockAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<EnemyThirdPartyFunctions> Data;
    private Animator _anim;
    private bool _onBlockComplete;

    private CancellationTokenSource _blockCts;
    
    protected override Status OnStart()
    {
        _anim = Self.Value.GetComponentInChildren<Animator>();
        _anim.Play("Block");

    //    Data.Value.isBlocking = false;
        
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

