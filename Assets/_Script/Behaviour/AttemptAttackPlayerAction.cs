using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttemptAttackPlayer", story: "[Self] initiates attack on [Player]", category: "Action", id: "c1b3b29f30c6bce421db8e16a07df3af")]
public partial class AttemptAttackPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<AudioClip> attackSound;

    private Animator _enemyAnim;
    private AudioSource _audioSource;
    private bool _attackFinished;

    protected override Status OnStart()
    {
        _attackFinished = false;
        NewMonoBehaviourScript.OnAttackAnimEnded += HandleAttackEnded;

        _enemyAnim = Self.Value.GetComponentInChildren<Animator>();
        _audioSource = Self.Value.GetComponent<AudioSource>();

        if (_enemyAnim && _audioSource != null)
        {
            int animIndex = Random.Range(1, 3);
            _enemyAnim.Play("Attack " + animIndex);
            _audioSource.PlayOneShot(attackSound);
        }
        else
        {
            Debug.Log("Animator or AudioSource is missing!");
            return Status.Failure; // don't sit in Running forever if refs are missing
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return _attackFinished ? Status.Success : Status.Running;
    }

    private void HandleAttackEnded()
    {
        _attackFinished = true;
    }

    protected override void OnEnd()
    {
        NewMonoBehaviourScript.OnAttackAnimEnded -= HandleAttackEnded;
    }
}

