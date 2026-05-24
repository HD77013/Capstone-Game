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

    public bool zoomIn;    
    
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
        
        Vector3 follow = Vector3.Lerp(Camera.position, playerPos, lerpSpeed * Time.deltaTime);
        
        if (shake > 0) {
            Vector3 cam = Random.insideUnitSphere * shakeAmount;
            transform.position = follow + new Vector3(cam.x * follow.x, cam.y * follow.y, -1f);
            shake -= Time.deltaTime * decreaseFactor;

        } else {
            shake = 0f;
            transform.position = follow;
        }

        if (zoomIn)
        {
            if (notReachedVal)
            {
                cam.orthographicSize = Mathf.MoveTowards(
                    cam.orthographicSize, 
                    zoomVal, 
                    zoomRate * Time.deltaTime
                );
        
                if (cam.orthographicSize <= zoomVal)
                {
                    notReachedVal = false;
                }
            }
        }
        else
        {
            // Reset when alive
            if (!notReachedVal || cam.orthographicSize != intialSize)
            {
                cam.orthographicSize = intialSize;
                notReachedVal = true;
            }
        }
    }
}
