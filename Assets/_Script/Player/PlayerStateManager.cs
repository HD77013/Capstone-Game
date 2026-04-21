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

    public bool isMoving => movement.action.IsPressed();
    public bool isJumping => jumping.action.IsPressed();
    public bool isAttacking => attack.action.IsPressed();

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
        currentState?.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }

    private void OnDrawGizmos()     // For hitboxes
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
