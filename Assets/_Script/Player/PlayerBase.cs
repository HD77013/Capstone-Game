using UnityEngine;

public abstract class PlayerBase
{
    public virtual bool Grounded(PlayerStateManager state)
    {
        return Physics2D.BoxCast(state.player.transform.position, state.boxSize, 0, -state.player.transform.transform.up, state.castDistance, state.groundLayer);
    }
        
    public abstract void EnterState(PlayerStateManager state);
    
    public abstract void UpdateState(PlayerStateManager state);

    public virtual void ExitState(PlayerStateManager state) { }
}
