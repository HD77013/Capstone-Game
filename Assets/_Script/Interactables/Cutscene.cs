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
            ShowCutscene();
        }
    }

    void ShowCutscene()
    {
        camera.onCutscene = true;
        camera.zoom = true;

        camera.zoomVal = 7.0f;
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
