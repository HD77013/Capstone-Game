using UnityEngine;

public class PlayerIdle : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        state.animator.SetBool("Walking", false);
    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        if (state.input.isMoving)
            state.SwitchState(PlayerStateType.Walk);
        
        if (state.input.JumpPressed && Grounded(state))
            state.SwitchState(PlayerStateType.Jumping);
        
        if (state.input.AttackPressed && Grounded(state))
            state.SwitchState(PlayerStateType.Attack);
        
        if (state.input.isBlocking && Grounded(state))
            state.SwitchState(PlayerStateType.Blocking);
        
    }
}
