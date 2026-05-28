using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStateType
{
    Idle, 
    Walk, 
    Jumping, 
    Attack, 
    Damaged, 
    Blocking,
}
public class PlayerStateManager : MonoBehaviour
{
    private PlayerBase currentState;
    public PlayerScript data;
    public PlayerDeath death;
    public ComboScript combo;
    public PlayerInput input;
    public FlashScript flash;
    
    private Dictionary<PlayerStateType, PlayerBase> stateDictionary;

    [Header("Shared Player Components")] 
    public GameObject player;
    public Rigidbody2D pRb2d;
    public Animator animator;
    public float jumpForce;
    public float walkSpeed;
    public AudioSource audio;
    
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    
    [Header("Keybinds")]
    public InputActionReference movement;
    public InputActionReference jumping;
    public InputActionReference attack;

    [Header("State checking")]
    public bool onCombo;

    public bool IsBlocking; 
    
    [Header("Knockback Data")]
    public Transform knockbackSource;
    public float knockbackForce;
    public float knockbackDuration;

    [Header("Death")] public bool isDead;
    void Awake()
    {
        stateDictionary = new Dictionary<PlayerStateType, PlayerBase>()
        {
            { PlayerStateType.Idle, new PlayerIdle() },
            { PlayerStateType.Walk, new PlayerWalking() },
            { PlayerStateType.Jumping, new PlayerJumping() },
            { PlayerStateType.Attack, new PlayerAttacking() },
            { PlayerStateType.Blocking, new PlayerBlocking() },
            { PlayerStateType.Damaged , new PlayerDamage() }
        };
        
        SwitchState(PlayerStateType.Idle);
    }
    
    void Update() => currentState.UpdateState(this);   
    
    void FixedUpdate() => currentState.FixedUpdateState(this);
    

    public void SwitchState(PlayerStateType newStateType)       // Looking up for state in dictionary when called
    {
        SwitchState(stateDictionary[newStateType]);
    }

    public void SwitchState(PlayerBase state)       // Actually switches to that state while exiting out of the other
    {
        Debug.Log("State switched " + state);
        currentState?.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }
    
    // To check if player's hp will drop to 0. If so, they transition to death state. But not, they will only transition to hurt state
    public void OnDamageTaken(Transform source, float force, float duration)
    {
        if (data.health <= 0)
        {
            currentState?.ExitState(this);
            isDead = true;
            input.isEnabled = false;
            death.OnDeath(source, force, duration);
        }
        else
        {
            knockbackSource = source;
            knockbackForce  = force;
            knockbackDuration = duration;
            
            SwitchState(PlayerStateType.Damaged);

            
        }
        
    }

    public void StopPlayer()
    {
        currentState?.ExitState(this);
        SwitchState(PlayerStateType.Idle);
        input.isEnabled = false;
    }

    private void OnDrawGizmos()     // For hitboxes
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
