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

        SetEnemyChase(false);
        SetEnemyHostile(false);
    }

    protected override IEnumerator PlayCutscene()
    {
        // Player view
        SetCamSize(3f, 6.5f);
        camera.followPlayer = true;
        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);

        SetEnemyChase(true);
        player.SwitchState(PlayerStateType.Idle);

        yield return new WaitForSeconds(0.5f);

        SetEnemyChase(false);

        yield return new WaitForSeconds(2.0f);

        // Enemy view
        AdjustCam(camPos, 7.0f, 6.5f);
        camera.followPlayer = false;

        yield return new WaitForSeconds(0.7f);

        SetEnemyChase(false);
        SetEnemyHostile(false);

        yield return new WaitForSeconds(2.7f);

        // Quick cut: player → enemy
        SetCamSize(3.0f, 6.5f);
        camera.followPlayer = true;

        yield return new WaitForSeconds(0.7f);

        AdjustCam(camPos, 3.0f, 6.5f);
        camera.followPlayer = false;

        yield return new WaitForSeconds(0.7f);

        SetEnemyChase(true);

        yield return new WaitForSeconds(0.7f);

        // Back to player
        camera.followPlayer = true;
        player.move = Vector2.right;
        player.SwitchState(PlayerStateType.Walk);

        yield return new WaitForSeconds(1.5f);

        SetEnemyHostile(true);
        EndCutscene();
    }

    private void SetEnemyChase(bool canChase)
    {
        foreach (EnemyScript e in scene.npcs)
            e.canChase = canChase;
    }

    private void SetEnemyHostile(bool hostile)
    {
        foreach (EnemyScript e in scene.npcs)
        {
            e.allowHostile = hostile;
            e.wandering = hostile;
        }
    }
}