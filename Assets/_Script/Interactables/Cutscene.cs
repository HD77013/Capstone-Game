using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    private PlayerStateManager player;

    private bool cutsceneTriggered;

    private Coroutine cutRoutine;
    
    public CameraScript camera;

    [Header("Modifiable Variables")]
    public float cutsceneTime;
    public Vector3 camPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (camera == null) camera = GameObject.FindAnyObjectByType(typeof(CameraScript)) as CameraScript;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = GameObject.Find("Player").GetComponent<PlayerStateManager>();

        if (player != null && !cutsceneTriggered)
        {
            cutsceneTriggered = true;
            AdjustCam();

            player.input.isEnabled = false;
            
            if (cutRoutine != null)
                StopCoroutine(cutRoutine);

            cutRoutine = StartCoroutine(cutsceneDuration());
        }
    }

    // Camera work for cutscene
    void AdjustCam()
    {
        camera.onCutscene = true;
        camera.zoomVal = 11.0f;
        camera.zoomRate = 6.5f;

        camera.zoomToInitial = true;
        camera.zoom = true;
        camera.camDestination = new Vector3(camPos.x, camPos.y, -1f);
    }

    private IEnumerator cutsceneDuration()
    {
        
        yield return new WaitForSeconds(cutsceneTime);
        camera.onCutscene = false;
        camera.zoom = false;
        player.input.isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, camPos);
    }
}
