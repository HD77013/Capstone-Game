using UnityEngine;

public class Cutscene : MonoBehaviour
{
    private PlayerScript player;
    
    public CameraScript camera;

    public Vector3 camPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (camera == null) camera = GameObject.FindAnyObjectByType(typeof(CameraScript)) as CameraScript;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (player != null)
        {
            AdjustCam();
        }
    }

    // Camera work for cutscene
    void AdjustCam()
    {
        camera.onCutscene = true;
        camera.zoomVal = 11.0f;
        camera.zoomRate = 6.5f;

        camera.zoom = true;
        camera.camDestination = new Vector3(camPos.x, camPos.y, -1f);
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
