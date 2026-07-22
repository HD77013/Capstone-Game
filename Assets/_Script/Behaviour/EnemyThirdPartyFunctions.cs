using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyThirdPartyFunctions : MonoBehaviour
{
    public Rigidbody2D enemyRB;

    public GameObject player;
    [SerializeField]private PlayerScript _playerScript;
    [SerializeField]private PlayerStateManager _player;
    private ComboScript _combo;
    private FlashScript _flash;
    
    public LayerMask players;
    
    [SerializeField] private BloodScript blood;
    
    [Header("Attack Logic")]
    public GameObject attackPoint;
    [SerializeField]private bool isAttacking;
    public float castDistance;
    public Vector2 atkBox;
    [SerializeField] private float cooldown;
    public int Damage;
    
    [SerializeField] private bool centered;
    
    public float minWalkTime, maxWalkTime;

    private float facingDirection = -1; // 1 right, -1 left

    private float randomTime, timer;
    public bool isWalking = false;

    [Header("Sounds")]
    public AudioSource soundManager;
    public AudioClip hitSound;
    
    [Header("Separation")]
    public LayerMask enemyLayer;
    public float separationRadius = 1.2f;
    public float separationForce = 3f;
    
    [Header("Chase")]
    [SerializeField]private float chaseTargetOffset; // Set once in Start()
    
    [SerializeField] private Vector2 follow;

    [SerializeField] private bool isKnockedBack;

    public bool isDead;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            _player = player.GetComponent<PlayerStateManager>();
            _playerScript = player.GetComponent<PlayerScript>();
            _flash = GetComponent<FlashScript>();
            _combo = player.GetComponent<ComboScript>();
        }
        
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        
        timer = Random.Range(0f, randomTime);
        chaseTargetOffset = Random.Range(-1.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        
        if (_player.isDead) return;
        
        
        if (isKnockedBack) return;
        
        CheckAttack();
        ApplyFacing();
    }

    private void FixedUpdate()
    { 
        if (isDead) return;
        
        float targetX = (player.transform.position.x + chaseTargetOffset) - transform.position.x;
        follow = new Vector2(targetX, player.transform.position.y - transform.position.y);

        if (Mathf.Abs(follow.x) > 0.15f) {
            facingDirection = follow.x < 0 ? -1f : 1f;
        }
        
      //  enemyRB.linearVelocity = new Vector2(enemyRB.linearVelocity.x * facingDirection, enemyRB.linearVelocity.y);
        
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
        
        enemyRB.linearVelocity = Vector2.zero; // Cancel existing movement
        enemyRB.linearVelocity = new Vector2(knockbackDir.x * knockbackForce, 0f);
        
        yield return new WaitForSeconds(duration);
        
        isKnockedBack = false;
    }

    public bool CheckAttack()
    {
        return Physics2D.BoxCast(attackPoint.transform.position, atkBox, 0, transform.forward, castDistance, players);
    }

    // Activated via animation event
    public void ActivatePlrDMG()
    {
        if (CheckAttack())
        {
            if (_player.IsBlocking)
            {
                _playerScript.TakeDamage(0, transform, 30f, 0.4f);
                _playerScript.DepleteEnergy(5);
            }
            else
            {
                soundManager.PlayOneShot(hitSound);
                _playerScript.PlayBlood(transform);

                _playerScript.TakeDamage(Damage, transform, 30f, 0.4f);
            }

            // Hand off all three parameters through the state manager // Debating whether this should be revised or not
            // _player.TriggerKnockback(transform, 30f, 0.4f);
        }
    }
    
    public Vector2 GetSeparationVelocity()
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
}
