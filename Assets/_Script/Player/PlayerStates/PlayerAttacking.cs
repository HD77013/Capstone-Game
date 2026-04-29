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
        if (state.input.AttackPressed)
        {
            if (state.combo.canCombo && !state.combo.OnComboCooldown)
            {
                Debug.Log("Next");
                state.combo.ComboStep();
            }
            else
                state.combo.InputBuffer = true;
        }


        if (!state.onCombo)
        {
            state.SwitchState(PlayerStateType.Idle);
        }
    }
}
