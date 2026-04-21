using UnityEngine;

public class PlayerJumping : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        state.pRb2d.AddForce(new Vector2(state.pRb2d.linearVelocity.x, state.jumpForce));
    }
    
    public override void UpdateState(PlayerStateManager state)
    {   
        if (state.isMoving)
        {
            state.SwitchState(PlayerStateType.Walk);
        }
        else
        {
            state.SwitchState(PlayerStateType.Idle);
        }
    }
}
