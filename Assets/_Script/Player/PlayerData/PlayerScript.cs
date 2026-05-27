using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public int health;
    public int maxHealth;
    public float baseDamage;
    public int energy;
    public int maxEnergy;

    public bool isRestoringEnergy;
    
    
    
    [Header("UI")] 
    public Sprite[] HP;
    public Sprite[] Energy;

    public Image healthDisplay;
    public Image energyDisplay;
    
    public CanvasGroup damageScreen;

    private float energyRestoreTimer = 0f;
    public float energyRestoreInterval = 1.0f;
    
    public void Update()
    {

        if (energy < maxEnergy && !isAttacking)
        {
            energyRestoreTimer += Time.deltaTime;

            if (energyRestoreTimer >= energyRestoreInterval)
            {
                energyRestoreTimer = 0;
                energy++;
                UpdateEnergyDisplay();
                
            }
        }
        else if (isAttacking)
        {
            energyRestoreTimer = 0;
        }
    }

    public void DepleteEnergy()
    {
        energy--;
        UpdateEnergyDisplay();
    }

    public bool EnoughEnergy()
    {
        return energy >= 5;
    }

    public void ResetAttributes()
    {
        health = maxHealth;
        damageScreen.alpha = 0;
        healthDisplay.sprite = HP[0];
        energyDisplay.sprite = Energy[0];
    }

    void UpdateEnergyDisplay()
    {
        if (!state.isDead)
        {
            switch (energy)
            {
                case 25:
                    energyDisplay.sprite = Energy[0];
                    break;
                case 20:
                    energyDisplay.sprite = Energy[1];
                    break;
                case 15:
                    energyDisplay.sprite = Energy[2];
                    break;
                case 10:
                    energyDisplay.sprite = Energy[3];
                    break;
                case 5:
                    energyDisplay.sprite = Energy[4];
                    break;
            }
        }
    }

    // Called by hurt state
    public void TakeDamage(int amount, Transform source, float force, float duration)
    {
        health -= amount;
        
        state.OnDamageTaken(source, force, duration);
        
        if (!state.isDead)
        {
            switch (health)
            {
                case 10:
                    damageScreen.alpha = 0;
                    healthDisplay.sprite = HP[0];
                    break;
                case 8:
                    damageScreen.alpha = 0.1f;
                    healthDisplay.sprite = HP[1];
                    break;
                case 6:
                    damageScreen.alpha = 0.2f;
                    healthDisplay.sprite = HP[2];
                    break;
                case 4:
                    damageScreen.alpha = 0.3f;
                    healthDisplay.sprite = HP[3];
                    break;
                case 2:
                    damageScreen.alpha = 0.4f;
                    healthDisplay.sprite = HP[4];
                    break;
            }
        }
    }
    
    // Will be actived by anim event
    public void Attack()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(attackPoint.transform.position, atkBox, 0, enemies);

        foreach (Collider2D enemyGameObject in enemy)
        {
            EnemyScript enemyScript = enemyGameObject.GetComponent<EnemyScript>();
            
            if (enemyScript != null && !enemyScript.isDead)
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

                if (!enemyScript.isDead)
                {
                    StartCoroutine(enemyScript.Knockback(transform, 30f, knockbackCooldown));
                }

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
