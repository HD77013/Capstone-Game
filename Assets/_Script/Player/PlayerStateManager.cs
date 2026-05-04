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
    
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    
    [Header("Keybinds")]
    public InputActionReference movement;
    public InputActionReference jumping;
    public InputActionReference attack;

    [Header("State checking")]
    public bool onCombo;
    public bool IsBlocking { get; internal set; }
    
    [Header("Knockback Data")]
    public Transform knockbackSource;
    public float knockbackForce;
    public float knockbackDuration;
    
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
    
    void Update()
    {
        currentState.UpdateState(this);   
    }

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
    
    public void TriggerKnockback(Transform source, float force, float duration)
    {
        knockbackSource = source;
        knockbackForce  = force;
        knockbackDuration = duration;
        SwitchState(PlayerStateType.Damaged);
    }

    private void OnDrawGizmos()     // For hitboxes
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
