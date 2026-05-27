using UnityEngine;

public class PlayerAttacking : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        if (state.combo.comboStep == 0 && !state.combo.OnComboCooldown)
        {
            state.combo.StartCombo();
            state.data.DepleteEnergy();
        }

    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        if (state.input.AttackPressed)
        {
            if (state.combo.canCombo && !state.combo.OnComboCooldown)
            {
                Debug.Log("Next");
                state.combo.ComboStep();
                state.data.DepleteEnergy();
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
