using System.Collections;
using UnityEngine;

public class HallwayCutscene : Cutscene
{
    protected override void Start()
    {
        base.Start();
        BeginCutscene();
    }

    protected override IEnumerator PlayCutscene()
    {
        camera.followPlayer = true;

        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(3.5f);

        player.SwitchState(PlayerStateType.Idle);

        yield return new WaitForSeconds(1.5f);

        EndCutscene();
    }
}