using System.Collections;
using UnityEngine;

public class HallwayCutscene : Cutscene
{
    public Vector2 camPos;
    public NextLVL scene;
    
    protected override void Start()
    {
        base.Start();
        BeginCutscene();

        scene = GameObject.Find("Next Level").GetComponent<NextLVL>();
    }

    protected override IEnumerator PlayCutscene()
    {
        // Player view
        AdjustCam(Vector2.zero, 3f, 6.5f);
        
        camera.followPlayer = true;
        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);

        if (scene.npcs.Count > 0)
        {
            foreach (EnemyScript current in scene.npcs)
            {
                current.canChase = true;
                current.canAttack = false;
            }
        }

        player.SwitchState(PlayerStateType.Idle);

        yield return new WaitForSeconds(2.5f);

        // Enemy view
        camera.followPlayer = false;
        AdjustCam(camPos, 3f, 6.5f);

        yield return new WaitForSeconds(0.7f);

        if (scene.npcs.Count > 0)
        {
            foreach (EnemyScript current in scene.npcs)
            {
                current.canChase = false;
                current.canAttack = false;
                current.wandering = false;
            }
        }

        yield return new WaitForSeconds(2.7f);

        // Player view
        camera.followPlayer = true;

        yield return new WaitForSeconds(0.7f);

        // Enemy view
        camera.followPlayer = false;
        AdjustCam(camPos, 3f, 6.5f);

        camera.followPlayer = true;

        yield return new WaitForSeconds(0.7f);

        if (scene.npcs.Count > 0)
        {
            foreach (EnemyScript current in scene.npcs)
            {
                current.canChase = true;
            }
        }

        yield return new WaitForSeconds(0.7f);

        // Player view
        camera.followPlayer = true;
        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);

        EndCutscene();

        if (scene.npcs.Count > 0)
        {
            foreach (EnemyScript current in scene.npcs)
            {
                current.canAttack = true;
            }
        }
    }
}