using UnityEngine;

public class PlayerWalking : PlayerBase
{
    private Vector2 direction;
    public override void EnterState(PlayerStateManager state)
    {
        
    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        direction = state.movement.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(direction.x, 0, direction.y);
        
        if (move != Vector3.zero)
        {
            // Handles player orientation
            state.player.transform.localScale = new Vector3(move.x, 1f, 1f);
            
            state.animator.SetBool("Walking", true);
        }
        else
        {
            state.animator.SetBool("Walking", false);
            state.SwitchState(PlayerStateType.Idle);
        }
        
        Debug.Log(direction * state.walkSpeed);
        
        if (state.input.JumpPressed && Grounded(state))
            state.SwitchState(PlayerStateType.Jumping);
        
        if (state.input.AttackPressed && Grounded(state) && state.data.EnoughEnergy())
            state.SwitchState(PlayerStateType.Attack);
        
        if (state.input.isBlocking && Grounded(state))
            state.SwitchState(PlayerStateType.Blocking);
        
    }
    
    public override void FixedUpdateState(PlayerStateManager state)
    {
        state.pRb2d.linearVelocity = new Vector2(direction.x * state.walkSpeed, state.pRb2d.linearVelocity.y);
    }
}
