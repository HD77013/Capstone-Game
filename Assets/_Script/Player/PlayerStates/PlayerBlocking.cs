using UnityEngine;

public class PlayerBlocking : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        state.IsBlocking = true;
    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        
    }

    public override void ExitState(PlayerStateManager state)
    {
        state.IsBlocking = false;
    }
}
