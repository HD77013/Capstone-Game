using System;
using System.Collections;
using UnityEngine;
public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D pRb2d;
    
    public Vector2 boxSize;
    public float castDistance;
    
    [Header("ReferencedScripts")]
    [SerializeField] private FlashScript flash;
    [SerializeField] private ComboScript combo;
    [SerializeField] private BloodScript blood;
    [SerializeField] private CameraScript camera;
    [SerializeField] private PlayerStateManager state;
    
    [Header("Attack Logic")]
    public GameObject attackPoint;
    public Vector2 atkBox;
    public float atkCastDis;
    public LayerMask enemies;
    public bool isAttacking;
    
    [Header("Knockback/Damage Logic (towards enemy)")]
    public float forwardForce;
    public float knockbackCooldown;
    
    [Header("Sounds")]
    public AudioSource soundManager;
    public AudioClip attackSound;
    public AudioClip[] hitSound;
    public AudioClip[] blockedSound;
    
    [Header("Basic attributes")]
    public float health;
    public float maxHealth;
    public float baseDamage;

    // Called by hurt state
    public void TakeDamage(float amount, Transform source, float force, float duration)
    {
        health -= amount;
        
        state.OnDamageTaken(source, force, duration);
    }
    
    // Will be actived by anim event
    public void Attack()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(attackPoint.transform.position, atkBox, 0, enemies);

        foreach (Collider2D enemyGameObject in enemy)
        {
            EnemyScript enemyScript = enemyGameObject.GetComponent<EnemyScript>();
            
            if (enemyScript != null)
            {
                if (enemyScript.isBlocking)
                {
                    AudioClip blocked = blockedSound[combo.comboStep];
                    soundManager.PlayOneShot(blocked);
                }
                else
                {
                    AudioClip hits = hitSound[combo.comboStep];
                    soundManager.PlayOneShot(hits);
                }
                
                camera.shake = 0.05f;
                
                enemyScript.Damaged(baseDamage);
                enemyScript.PlayBlood(transform);
                
                StartCoroutine(enemyScript.Knockback(transform, 30f, knockbackCooldown));
            }
        }
    }
    
    public void PlayBlood(Transform source) // Debate whether this should be moved to Player State
    {
        Vector2 attackerPos = ((Vector2)transform.position - (Vector2)source.position).normalized;
        Vector2 posDir = new Vector2(attackerPos.x, 0f).normalized;
        
        blood.SpawnBlood(transform, posDir);
    }
    
    public float GetFacingDirection()
    {
        return transform.localScale.x; // Returns 1 for right, -1 for left
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
        
        Gizmos.DrawWireCube(attackPoint.transform.position, atkBox);
    }
}
