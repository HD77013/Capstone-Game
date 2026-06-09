using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Camera;
    public Camera cam;
    public Transform player;

    public float shake;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 0.5f;

    public PlayerStateManager plrScript;

    public bool onCutscene;
    public bool followPlayer = true;

    public bool zoom;
    public bool zoomToInitial;

    public Vector3 camDestination;
    
    public float zoomRate = 1.0f;
    
    public bool notReachedVal = true;
    public float zoomVal = 3.0f;
    public float intialSize = 5.0f;
    
    [SerializeField]private float lerpSpeed = 1.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, -1f);
        
        if (!onCutscene || !followPlayer)
            camDestination = playerPos;
        

        if (zoom)
        {
            if (notReachedVal)
            {
                cam.orthographicSize = Mathf.MoveTowards(
                    cam.orthographicSize, 
                    zoomVal, 
                    zoomRate * Time.deltaTime
                );
        
                if (Mathf.Approximately(cam.orthographicSize, zoomVal))
                {
                    notReachedVal = false;
                }
            }
        }
        else if (!notReachedVal || !Mathf.Approximately(cam.orthographicSize, intialSize))
        {
            // Recommended to set to true once zoom is set to true by third party scripts
            if (zoomToInitial)
            {
                cam.orthographicSize = Mathf.MoveTowards(
                    cam.orthographicSize, 
                    intialSize, 
                    zoomRate * Time.deltaTime
                );
            }
            else
            {
                cam.orthographicSize = intialSize;
            }

            notReachedVal = true;
        }

        Vector3 follow = Vector3.Lerp(Camera.position, camDestination, lerpSpeed * Time.deltaTime);

        if (shake > 0)
        {
            Vector3 cam = Random.insideUnitSphere * shakeAmount;
            transform.position = follow + new Vector3(cam.x, cam.y, -1f);
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0f;
            transform.position = follow;
        }
    }
}
