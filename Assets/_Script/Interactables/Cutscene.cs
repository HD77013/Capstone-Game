using UnityEngine;

public class Cutscene : MonoBehaviour
{
    private PlayerScript player;
    
    public CameraScript camera;

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
            camera.onCutscene = true;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
