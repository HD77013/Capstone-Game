using System;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public EnemyScript enemy;
    public static event Action OnAttackAnimEnded;
    
    
    public void DamageCheck()
    {
        enemy.ActivatePlrDMG();
    }

    public void EndAttack()
    {
        OnAttackAnimEnded?.Invoke();
    }
}
