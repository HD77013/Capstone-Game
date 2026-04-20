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
    public Rigidbody2D pRb2d;
    public Animator animator;
    
    [Header("Keybinds")]
    public InputActionReference movement;
    public InputActionReference jumping;
    public InputActionReference attack;

    void Awake()
    {
        stateDictionary = new Dictionary<PlayerStateType, PlayerBase>()
        {
            { PlayerStateType.Idle, new PlayerIdle() },
            { PlayerStateType.Walk, new PlayerWalking() },
            { PlayerStateType.Jumping, new PlayerJumping() },
            { PlayerStateType.Attack, new PlayerAttacking() },
            { PlayerStateType.Blocking, new PlayerBlocking() }
        };
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

}
