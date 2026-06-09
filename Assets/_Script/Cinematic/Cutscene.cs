using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    private PlayerStateManager player;

    private Transform playerPos;

    private bool cutsceneTriggered;

    private Coroutine cutRoutine;
    
    public CameraScript camera;

    [Header("Modifiable Variables")] 
    public bool colliderRequired;   // Varies by scene
    
    public float cutsceneTime;
    public Vector3 camPos;
    public float camSize;
    public float camRate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (camera == null) camera = GameObject.FindAnyObjectByType(typeof(CameraScript)) as CameraScript;
        if (playerPos == null) playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliderRequired) return;
            
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
    public void AdjustCam()
    {
        camera.onCutscene = true;
        camera.followPlayer = false;
        camera.zoomVal = camSize;
        camera.zoomRate = camRate;

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
        camera.followPlayer = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, camPos);
    }
}
