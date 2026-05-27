using UnityEngine;

public class PlayerAttacking : PlayerBase
{
    public override void EnterState(PlayerStateManager state)
    {
        if (state.combo.comboStep == 0 && !state.combo.OnComboCooldown)
        {
            state.combo.StartCombo();
            state.data.DepleteEnergy(1);
        }

    }
    
    public override void UpdateState(PlayerStateManager state)
    {
        if (state.input.AttackPressed && state.data.EnoughEnergy())
        {
            if (state.combo.canCombo && !state.combo.OnComboCooldown)
            {
                Debug.Log("Next");
                state.combo.ComboStep();
                state.data.DepleteEnergy(1);
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
