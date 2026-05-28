using UnityEngine;

public class PlayerDamage : PlayerBase
{
    private float _knockbackTimer;

    public override void EnterState(PlayerStateManager state)
    {
        if (state.IsBlocking)
        {
            int random = Random.Range(0, state.data.blockedSound.Length);
            AudioClip block = state.data.blockedSound[random];
            
            state.audio.PlayOneShot(block);
            
            state.animator.Play("Block Reaction");
        }
        else
        {
            state.flash.Flash();
            state.animator.Play("Damage");
        }


        // Calculate and apply the knockback impulse
        Vector2 dir = ((Vector2)state.player.transform.position
                       - (Vector2)state.knockbackSource.position).normalized;

        state.pRb2d.linearVelocity = Vector2.zero;
        state.pRb2d.linearVelocity = new Vector2(dir.x * state.knockbackForce, 0f);

        _knockbackTimer = state.knockbackDuration;
    }

    public override void UpdateState(PlayerStateManager state)
    {
        _knockbackTimer -= Time.deltaTime;

        if (_knockbackTimer <= 0f)
        {
            state.SwitchState(PlayerStateType.Idle);

            if (state.IsBlocking)
            {
                Debug.Log("Returning to block stance");
                state.SwitchState(PlayerStateType.Blocking);
            }

        }
    }
}
