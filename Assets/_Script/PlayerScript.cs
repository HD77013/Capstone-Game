using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private FlashScript flash;
    [SerializeField] private ComboScript combo;
    [SerializeField] private BloodScript blood;
    [SerializeField] private CameraScript camera;
    
    public Rigidbody2D pRb2d;
    
    public InputActionReference movement;
    public InputActionReference jumping;
    public InputActionReference attack;
    
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    
    public Vector2 direction;
    
    public float speed;
    public float jump;

    public GameObject attackPoint;
    public Vector2 atkBox;
    public float atkCastDis;
    public LayerMask enemies;
    
    [Header("Sounds")]
    public AudioSource soundManager;
    public AudioClip attackSound;
    public AudioClip[] hitSound;
    
    [Header("Basic attributes")]
    public float Health;
    public float maxHealth;
    public float baseDamage;
    
    public Animator animator;
    
    public bool isKnockedBack;
    
    public float forwardForce;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flash = GetComponent<FlashScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack) return;
        
        direction = movement.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(direction.x, 0, direction.y);
        
        if (move != Vector3.zero)
        {
            transform.localScale = new Vector3(move.x, 1f, 1f);
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        if (jumping.action.WasPressedThisFrame() && Grounded())
        {
            Debug.Log("Jumping");
            pRb2d.AddForce(new Vector2(pRb2d.linearVelocity.x, jump));
        }


        if (attack.action.WasPressedThisFrame() && Grounded())
        {
            if (combo.comboStep == 0 && !combo.OnComboCooldown)
                combo.StartCombo();
            else if (combo.canCombo && combo.comboStep > 0 && !combo.OnComboCooldown)
                combo.ComboStep();
            else
                combo.InputBuffer = true;
        }
        
    }

    private void FixedUpdate()
    {
        pRb2d.AddForce(direction * speed);
    }

    public bool Grounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
    }
    
    // Will be actived by anim event (for now, the moment input is pressed)
    public void Attack()
    {
        
        Collider2D[] enemy = Physics2D.OverlapBoxAll(attackPoint.transform.position, atkBox, 0, enemies);

        foreach (Collider2D enemyGameObject in enemy)
        {
            EnemyScript enemyScript = enemyGameObject.GetComponent<EnemyScript>();
            
            if (enemyScript != null)
            {
                AudioClip hits = hitSound[combo.comboStep];
                soundManager.PlayOneShot(hits);
                
                camera.shake = 0.05f;
                
                enemyScript.Damaged(baseDamage);
                enemyScript.PlayBlood(transform);
                
                StartCoroutine(enemyScript.Knockback(transform, 30f, 0.4f));
                
                
            }
            
            
            
            Debug.Log("Enemy is hit!");
        }
    }

    public void Damage(float damage)
    {
        Health -= damage;
        
        flash.Flash();
        
        if (Health <= 0)
        {
            Debug.Log("Player is dead");
            
            Health = maxHealth;
        }
    }
    
    public void PlayBlood(Transform source)
    {
        Vector2 attackerPos = ((Vector2)transform.position - (Vector2)source.position).normalized;
        Vector2 posDir = new Vector2(attackerPos.x, 0f).normalized;
        
        blood.SpawnBlood(transform, posDir);
    }
    
    public IEnumerator Knockback(Transform source , float knockbackForce, float duration)
    {
        isKnockedBack = true;
        
        Vector2 directionToTarget = ((Vector2)transform.position - (Vector2)source.position).normalized;
        Vector2 knockbackDir = new Vector2(directionToTarget.x, 0f).normalized;
        
        pRb2d.linearVelocity = Vector2.zero; // Cancel existing movement
        pRb2d.linearVelocity = new Vector2(knockbackDir.x * knockbackForce, 0f);
        
        yield return new WaitForSeconds(duration);
        
        isKnockedBack = false;
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
