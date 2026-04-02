using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Camera;
    public Transform player;

    public float shake;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 0.5f;
    
    
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
        
    }
}
