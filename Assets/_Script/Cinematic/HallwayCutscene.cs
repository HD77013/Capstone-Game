using System.Collections;
using UnityEngine;

public class HallwayCutscene : Cutscene
{
    public Vector2 camPos;
    
    protected override void Start()
    {
        base.Start();
        BeginCutscene();
    }

    protected override IEnumerator PlayCutscene()
    {
        // Player view
        AdjustCam(Vector2.zero, 3f, 6.5f);
        
        camera.followPlayer = true;
        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);
        
        player.SwitchState(PlayerStateType.Idle);

        yield return new WaitForSeconds(0.7f);

        // Enemy view
        camera.followPlayer = false;
        AdjustCam(camPos, 3f, 6.5f);

        yield return new WaitForSeconds(2.7f);

        // Player view
        camera.followPlayer = true;

        yield return new WaitForSeconds(0.7f);

        // Enemy view
        camera.followPlayer = true;
        camera.followPlayer = false;
        AdjustCam(camPos, 3f, 6.5f);

        yield return new WaitForSeconds(0.7f);

        // Player view
        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);

        EndCutscene();
    }
}