using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public EnemyScript enemy;

    public void DamageCheck()
    {
        enemy.ActivatePlrDMG();
    }

    public void EndAttack()
    {
        enemy.EndAttack();
    }
}
