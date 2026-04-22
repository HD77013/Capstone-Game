using UnityEngine;

public class PlayerAttacking : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        if (state.combo.comboStep == 0 && !state.combo.OnComboCooldown)
            state.combo.StartCombo();

    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        if (state.isAttacking && state.combo.canCombo && state.combo.comboStep > 0 && !state.combo.OnComboCooldown)
        {
            Debug.Log("Next");
            state.combo.ComboStep();
        }
        else
            state.combo.InputBuffer = true; // Only set on real input

        if (!state.onCombo)
        {
            state.SwitchState(PlayerStateType.Idle);
        }
    }
}
