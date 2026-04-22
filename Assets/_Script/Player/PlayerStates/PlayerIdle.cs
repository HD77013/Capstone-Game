using UnityEngine;

public class PlayerIdle : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        state.animator.SetBool("Walking", false);
    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        if (state.isMoving)
        {
            state.SwitchState(PlayerStateType.Walk);
        }

        if (state.isJumping && Grounded(state))
        {
            state.SwitchState(PlayerStateType.Jumping);
        }

        if (state.isAttacking && Grounded(state))
        {
            state.SwitchState(PlayerStateType.Attack);
        }
    }
}
