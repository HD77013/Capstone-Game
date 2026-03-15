using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody2D enemyRB;

    public GameObject player;

    public Vector2 direction;
    
    public LayerMask players;
    
    [Header("Attack Logic")]
    public GameObject attackPoint;
    public Vector2 boxSize;
    public float castDistance;
    public Vector2 atkBox;
    public bool canAttack;

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

    private float facingDirection = -1;

    private float randomTime, timer;
    public bool isWalking = false;

    [Header("Basic Attributes")] 
    public float Health;
    public float Damage;
    public float speed;
    public float chaseSpeed;

    public Vector2 wanderPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        
        direction = Vector2.right * facingDirection;
        
        centered = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomTime) StateChange();
        
        CheckAttack();
        Attack();
        Patrol();
        ApplyFacing();
    }

    private void FixedUpdate()
    { 
        if (canChase)
        {
            // Chase player
            Vector2 follow = (player.transform.position - transform.position); // Player position
            enemyRB.linearVelocity = new Vector2(chaseSpeed * follow.x, enemyRB.linearVelocity.y); // Enemy follow player via x value but y is not affected
            wandering = false;
            centered = false;
            
            if (Mathf.Abs(follow.x) > 0.01f)
            {
                facingDirection = follow.x < 0 ? -1f : 1f;
            }
        }
        else if (wandering) // Only move during walk state, not pause state
        {
            enemyRB.linearVelocity = new Vector2(speed * facingDirection, enemyRB.linearVelocity.y);
        }
        else
        {
            enemyRB.linearVelocity = new Vector2(0, enemyRB.linearVelocity.y);
        }

    }

    public void Damaged(float damage)
    {
        Health -= damage;

        Debug.Log($"I have been damaged {damage}, remaining health: {Health}");
        
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        
        
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
        if (canAttack && CheckAttack())
        {
            Debug.Log("Player is is ranged");
            StartCoroutine(AttackingRoutine());
            
            canAttack = false;
        }

    }

    IEnumerator AttackingRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("Attack is activated");
        
        if (CheckAttack())
        {
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
