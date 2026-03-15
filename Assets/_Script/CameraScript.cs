using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Camera;
    public Transform player;
    
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
        
        transform.position = Vector3.Lerp(Camera.position, playerPos, lerpSpeed * Time.deltaTime);
        
    }
}
