using System.Collections;
using UnityEngine;

public class BigRoomCutscene : Cutscene
{
    public Vector2 camPos;
    private bool triggered;

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || !collision.CompareTag("Player")) return;
        
        camera.followPlayer = false;
        triggered = true;
        AdjustCam(camPos, 11f, 6.5f);
        BeginCutscene();
    }

    protected override IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(5.0f);
        EndCutscene();
    }
}