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
        AdjustCam(Vector2.zero, 3f, 6.5f);
        
        camera.followPlayer = true;

        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);
        
        AdjustCam(camPos, 3f, 6.5f);
        
        camera.followPlayer = false;
        
        player.SwitchState(PlayerStateType.Idle);

        yield return new WaitForSeconds(1.5f);

        EndCutscene();
    }
}