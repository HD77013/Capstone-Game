using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // New script will trigger dialogue interaction, usually if player's within range before it is never executed again

    public GameObject dialogue;
    public Transform canvas;
    
    public GameObject player;
    public PlayerStateManager script;
    public Camera cam;
    public CameraScript camera;
    
    public float detectionThreshold;
    private bool withinThreshold;

    private GameObject spawnedText;
    private bool dialogueActive = false;
    
    private float distance = float.MaxValue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        StartCoroutine(WaitForProximity());
    }
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        script = player.GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player.transform.position;
        distance = Vector2.Distance(transform.position, playerPos);
        
        // Continuously update dialogue position if active
        if (dialogueActive && spawnedText != null)
        {
            spawnedText.transform.position = cam.WorldToScreenPoint(transform.position);
        }
        
    }
    
    IEnumerator WaitForProximity()
    {
        while (distance > detectionThreshold)
        {
            yield return null; // wait a frame, then check again
        }
        

        ActivateDialogue(); // fires exactly once when condition is met
    }

    void ActivateDialogue()
    {
        Debug.Log("Dialogue begins!");

        camera.zoomVal = 4.0f;
        camera.zoom = true;
        script.StopPlayer();

        spawnedText = Instantiate(dialogue, canvas);
        dialogueActive = true;
        
        var temp = spawnedText.GetComponent<TextMeshProUGUI>();
        temp.transform.position = cam.WorldToScreenPoint(transform.position);
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionThreshold);
    }
}
