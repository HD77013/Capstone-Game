using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody2D enemyRB;

    public GameObject player;

    public Vector2 direction;
    
    public LayerMask players;
    
    private FlashScript flash;
    [SerializeField] private BloodScript blood;
    
    [Header("Attack Logic")]
    public GameObject attackPoint;
    public Vector2 boxSize;
    public float castDistance;
    public Vector2 atkBox;
    public bool canAttack;
    [SerializeField] private float cooldown;

    [Header("Chase Logic")]
    public float detectionThreshold;
    public bool canChase;

    [Header("Wander Logic")] 
    public bool wandering;

    public float leftPatrolX, rightPatrolX;
    public Vector2 patrolCenter;

    [SerializeField] private bool centered;
    
    public float minPauseTime, maxPauseTime;
    public float minWalkTime, maxWalkTime;

    private float facingDirection = -1; // 1 right, -1 left

    private float randomTime, timer;
    public bool isWalking = false;

    [Header("Sounds")]
    public AudioSource soundManager;
    public AudioClip attackSound;
    public AudioClip hitSound;
    
    [Header("Attack Cooldown")]
    public float attackCooldownDuration = 2.0f; // Time between attacks
    private float lastAttackTime;
    
    [Header("Basic Attributes")] 
    public float Health;
    public float Damage;
    public float speed;
    public float chaseSpeed;

    [SerializeField] private Vector2 follow;

    [SerializeField] private bool isKnockedBack;

    public Vector2 wanderPoints;
    
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flash = GetComponent<FlashScript>();
        
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        
        direction = Vector2.right * facingDirection;
        
        centered = true;
        
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomTime) StateChange();
        
        if (isKnockedBack)
        {
            Debug.Log("Enemy is in atk cooldown after knockback");
            cooldown = 10.0f;
            Debug.Log("Cooldown was " + cooldown);
        }
        
        if (isKnockedBack) return;
        
        CheckAttack();
        Attack();
        Patrol();
        ApplyFacing();
    }

    private void FixedUpdate()
    { 
        if (isKnockedBack) return; // Other movement logic is stops upon enemy being knocked back
        
        if (canChase)
        {
            // Chase player
             follow = (player.transform.position - transform.position); // Player position
            enemyRB.linearVelocity = new Vector2(chaseSpeed * follow.x, enemyRB.linearVelocity.y); // Enemy follow player via x value but y is not affected
            wandering = false;
            centered = false;
            
            if (Mathf.Abs(follow.x) > 0.01f)
            {
                facingDirection = follow.x < 0 ? -1f : 1f;
                animator.SetBool("Walking", true);
            }
        }
        else if (wandering) // Only move during walk state, not pause state
        {
            enemyRB.linearVelocity = new Vector2(speed * facingDirection, enemyRB.linearVelocity.y);
            animator.SetBool("Walking", true);
        }
        else
        {
            enemyRB.linearVelocity = new Vector2(0, enemyRB.linearVelocity.y);
            animator.SetBool("Walking", false);
        }

    }

    public void Damaged(float damage)
    {
        Health -= damage;
        
        flash.Flash();
        
      //  Debug.Log($"I have been damaged {damage}, remaining health: {Health}");
        
        if (Health <= 0)
        {
            Destroy(gameObject);
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
        
        lastAttackTime += 2.0f; // Add 2 seconds to cooldown
        
        Vector2 directionToTarget = ((Vector2)transform.position - (Vector2)source.position).normalized;
        Vector2 knockbackDir = new Vector2(directionToTarget.x, 0f).normalized;
        
        enemyRB.linearVelocity = Vector2.zero; // Cancel existing movement
        enemyRB.linearVelocity = new Vector2(knockbackDir.x * knockbackForce, 0f);
        
        yield return new WaitForSeconds(duration);
        
        isKnockedBack = false;
    }

    bool CheckAttack()
    {
        return Physics2D.BoxCast(attackPoint.transform.position, atkBox, 0, transform.forward, castDistance, players);
    }

    void StateChange()
    {
        wandering = !wandering;
        randomTime = wandering ? Random.Range(minWalkTime, maxWalkTime) : Random.Range(minPauseTime, maxPauseTime);
        
        timer = 0;
    }

    void Patrol()
    {
        Vector2 playerPos = player.transform.position;
        float distance = Vector2.Distance(transform.position, playerPos);

        canChase = distance < detectionThreshold;

        if (!canChase)
        {
            CenterPatrol();
            
            // Flip direction at patrol boundaries
            if (centered && (transform.position.x >= rightPatrolX + patrolCenter.x || transform.position.x <= leftPatrolX + patrolCenter.x))
            {
                Debug.Log("Flip direction");
                facingDirection *= -1;
                timer = 0;
                randomTime = Random.Range(minWalkTime, maxWalkTime);
            }
        }
    }

    void CenterPatrol()
    {
        if (!centered)
        {
            patrolCenter = transform.position;
            centered = true;
        }
    }
    
    private void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldownDuration && canAttack && CheckAttack())
        {
            Debug.Log("Cooldown was " + cooldown);
            soundManager.PlayOneShot(attackSound);
            
            Debug.Log("Player is is ranged");
            StartCoroutine(AttackingRoutine());
            
            lastAttackTime = Time.time;
            canAttack = false;
        }


    }

    IEnumerator AttackingRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("Attack is activated");
        
        if (CheckAttack())
        {
            soundManager.PlayOneShot(hitSound);
            
            PlayerScript script = player.GetComponent<PlayerScript>();
            script.Damage(Damage);
            script.PlayBlood(transform);
            
            StartCoroutine(script.Knockback(transform, 30f, 0.4f));
            
            Debug.Log("Player is damaged");
        }
        
        yield return new WaitForSeconds(0.5f);
        
        canAttack = true;
    }
    
    void ApplyFacing()
    {
        transform.localScale = new Vector3(facingDirection, 1f, 1f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.transform.position+transform.forward * castDistance, atkBox);
        Gizmos.DrawWireSphere(transform.position, detectionThreshold);
    }
}
