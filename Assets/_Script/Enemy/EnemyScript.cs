using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody2D enemyRB;

    public GameObject player;
    [SerializeField]private PlayerScript _playerScript;
    [SerializeField]private PlayerStateManager _player;
    private ComboScript _combo;

    public Vector2 direction;
    
    public LayerMask players;
    
    private FlashScript flash;
    [SerializeField] private BloodScript blood;
    
    [Header("Attack Logic")]
    public GameObject attackPoint;
    public Vector2 boxSize;
    [SerializeField]private bool isAttacking;
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
    private float pendingAttackDelay = -1f; // -1 means no attack is pending
    
    [Header("Basic Attributes")] 
    public float Health;
    public float Damage;
    public float speed;
    public float chaseSpeed;

    [Header("Block")] 
    public int blockChance;
    private bool isDamaged;
    public bool isBlocking;
    public bool hasBlockedThisCombo;

    [Header("Animation")] 
    public Animator animator;
    private int attackAnim;
    
    [Header("Separation")]
    public LayerMask enemyLayer;
    public float separationRadius = 1.2f;
    public float separationForce = 3f;
    
    [Header("Chase")]
    [SerializeField]private float chaseTargetOffset; // Set once in Start()
    [SerializeField]private float distanceTargetOffset;
    [SerializeField] private float AwayDir;
    
    [SerializeField] private Vector2 follow;

    [SerializeField] private bool isKnockedBack;

    public bool isDead;
    
    public Vector2 wanderPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flash = GetComponent<FlashScript>();
        _playerScript = player.GetComponent<PlayerScript>();
        _combo = player.GetComponent<ComboScript>();
        
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        
        timer = Random.Range(0f, randomTime);
        wandering = Random.value > 0.5f;
        attackCooldownDuration += Random.Range(-0.4f, 0.4f);
        chaseTargetOffset = Random.Range(-1.5f, 1.5f);
        distanceTargetOffset = Random.Range(2.5f, 5.0f);
        
        direction = Vector2.right * facingDirection;
        
        centered = true;
        
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        
        if (_player.isDead) return;

        if (timer >= randomTime) StateChange();
        
        if (isKnockedBack) return;
        
        CheckAttack();
        Attack();
        Patrol();
        ApplyFacing();
    }

    private void FixedUpdate()
    { 
        if (isDead) return;
        
        if (_combo.comboStep == 0)
        {
            // Resets block behabiour if player's combo resets
            hasBlockedThisCombo = false;
            isBlocking = false;
        }
        
        if (_playerScript.isAttacking && _combo.comboStep >= 1 && !hasBlockedThisCombo)
        {
            hasBlockedThisCombo = true;
            
            blockChance = Random.Range(0, 101);

            if (blockChance <= 20)
            {
                isBlocking = true;
                
                Debug.Log("Enemy has blocked");

                if (isDamaged)
                {
                    isDamaged = false;
                    isBlocking = false;
                    

                }
            }
            else
            {
                isDamaged = false;
                isBlocking = false;
            }
        }
        
        if (isKnockedBack) return; // Other movement logic is stops upon enemy being knocked back
        
        if (canChase)
        {
            float targetX = (player.transform.position.x + chaseTargetOffset) - transform.position.x;
            follow = new Vector2(targetX, player.transform.position.y - transform.position.y);

            wandering = false;
            centered = false;

            Vector2 sep = GetSeparationVelocity();
            float dir = Mathf.Sign(follow.x);

            // Use raw player distance for back-away, not the offset-adjusted follow
            AwayDir = Mathf.Abs(player.transform.position.x - transform.position.x);
            float awayDir = -Mathf.Sign(player.transform.position.x - transform.position.x);

            if (AwayDir < distanceTargetOffset && !isAttacking && !canAttack)
            {
                // Too close to the actual player — back away from them directly
                enemyRB.linearVelocity = new Vector2(chaseSpeed * awayDir + sep.x, enemyRB.linearVelocity.y);
                animator.SetBool("Walking", true);
            }
            else if (AwayDir >= distanceTargetOffset)
            {
                enemyRB.linearVelocity = new Vector2(sep.x, enemyRB.linearVelocity.y);
                animator.SetBool("Walking", false);
            }
            
            if (isAttacking || canAttack)
            {
                // Move toward offset target position normally
                enemyRB.linearVelocity = new Vector2(chaseSpeed * dir + sep.x, enemyRB.linearVelocity.y);
                animator.SetBool("Walking", true);
            }

            if (Mathf.Abs(follow.x) > 0.001f)
            {
                facingDirection = follow.x < 0 ? -1f : 1f;
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
        isDamaged = true;

        if (!isBlocking)
        {
            Health -= damage;
        
            flash.Flash();
            
            animator.Play("Damage");
        }
        else
        {

            
            Health -= damage / 2;

            isBlocking = false;
            
            animator.Play("Block");
        }
        
        
        if (Health <= 0)
        {
            isDead = true;
            animator.SetBool("Dead", true);

            float knockBackForce = Random.Range(999,999);

            PlayBlood(player.transform);
            StartCoroutine(Knockback(player.transform, knockBackForce, 2f));

            Destroy(gameObject, 5f);
        }
        
    }
    
    public void PlayBlood(Transform source)
    {
        Vector2 attackerPos = ((Vector2)transform.position - (Vector2)source.position).normalized;
        Vector2 posDir = new Vector2(attackerPos.x, 0f).normalized;

        if (!isBlocking)
        {
            blood.SpawnBlood(transform, posDir);
        }

    }
    
    public IEnumerator Knockback(Transform source , float knockbackForce, float duration)
    {
        isKnockedBack = true;
        
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
        if (Time.time - lastAttackTime >= attackCooldownDuration)
            canAttack = true;


        if (canAttack && CheckAttack())
        {
            if (pendingAttackDelay < 0f)
            {
                // Queue the attack with a random delay instead of firing instantly
                pendingAttackDelay = Random.Range(0f, 0.6f);
            }
        }

        if (pendingAttackDelay >= 0)
        {
            pendingAttackDelay -= Time.deltaTime;

            if (pendingAttackDelay <= 0)
            {
                pendingAttackDelay = -1f;

                if (CheckAttack())
                {
                    soundManager.PlayOneShot(attackSound);
            
                    attackAnim = Random.Range(1, 3);
                    animator.Play("Attack " + attackAnim);
            
                    lastAttackTime = Time.time;
                    canAttack = false;
                    isAttacking = true;
                }
            }
        }
    }

    // Activated via animation event
    public void ActivatePlrDMG()
    {
        if (CheckAttack())
        {
            if (!_player.IsBlocking)
            {
                soundManager.PlayOneShot(hitSound);
                _playerScript.PlayBlood(transform);
            }
            
            _playerScript.TakeDamage(Damage, transform, 30f, 0.4f);

            // Hand off all three parameters through the state manager // Debating whether this should be revised or not
            // _player.TriggerKnockback(transform, 30f, 0.4f);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
    
    Vector2 GetSeparationVelocity()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);
        Vector2 separation = Vector2.zero;

        foreach (var col in nearby)
        {
            if (col.gameObject == gameObject) continue;

            Vector2 away = (Vector2)transform.position - (Vector2)col.transform.position;
            // Weight by proximity — closer enemies push harder
            float weight = 1f - (away.magnitude / separationRadius);
            separation += away.normalized * weight;
        }

        return separation * separationForce;
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
