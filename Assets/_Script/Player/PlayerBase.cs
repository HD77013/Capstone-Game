using UnityEngine;

public abstract class PlayerBase
{
    public abstract void EnterState(PlayerStateManager state);
    
    public abstract void UpdateState(PlayerStateManager state);

    public virtual void ExitState(PlayerStateManager state) { }
}
