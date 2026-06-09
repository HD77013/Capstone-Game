using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    protected PlayerStateManager player;
    public CameraScript camera;

    protected virtual void Start()
    {
        if (camera == null) camera = FindAnyObjectByType<CameraScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateManager>();
    }

    protected void BeginCutscene()
    {
        player.input.isEnabled = false;
        player.onCutscene = true;
        StartCoroutine(PlayCutscene());
    }

    protected void EndCutscene()
    {
        player.onCutscene = false;
        player.input.isEnabled = true;
        camera.followPlayer = true;
        camera.zoom = false;
    }

    // Each cutscene scene overrides this
    protected virtual IEnumerator PlayCutscene()
    {
        yield break;
    }

    public void AdjustCam(Vector3 pos, float size, float rate)
    {
        camera.zoomVal = size;
        camera.zoomRate = rate;
        camera.zoomToInitial = true;
        camera.zoom = true;
        camera.camDestination = new Vector3(pos.x, pos.y, -1f);
    }

    public void SetCamSize(float size, float rate)
    {
        camera.zoomVal = size;
        camera.zoomRate = rate;
        camera.notReachedVal = true;
        camera.zoom = true;
        camera.zoomToInitial = false;
    }
}