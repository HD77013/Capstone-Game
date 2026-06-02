using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLVL : MonoBehaviour
{
    [SerializeField]private bool allDead;
    [SerializeField]private bool canMoveOn;
    private PlayerScript player;

    public List<EnemyScript> npcs = new List<EnemyScript>();
    
    public string newScene;
    
    public Vector3 nextTransformPos;

    public GameObject arrow;

    public GameObject canvas;
    public GameObject text;
    private TextMeshProUGUI textMesh;

    [SerializeField]private InputActionReference next;

    void Start()
    {
        if (canvas == null) canvas = GameObject.FindGameObjectWithTag("UI");

        if (canvas != null)
        {
            foreach (Transform child in canvas.transform)
            {
                if (child.tag == "Notif")
                    text = child.gameObject;
            }

            if (text != null) textMesh = text.GetComponent<TextMeshProUGUI>();
        }

        FindAllNPCs();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        
        if (player != null)
        {
            canMoveOn = true;

            if (allDead)
            {
                text.SetActive(true);
                textMesh.text = "Press ENTER";
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player != null)
        {
            canMoveOn = false;
            
            text.SetActive(false);
        }
    }

    // Finds all EnemyScript instances in the scene
    public void FindAllNPCs()
    {
        npcs.Clear();

        EnemyScript[] found = FindObjectsByType<EnemyScript>(FindObjectsSortMode.None);

        npcs.AddRange(found);
    }

    public void RequestRemove(EnemyScript npc)
    {
        npcs.Remove(npc);

        if (npcs.Count == 0)
        {
            allDead = true;
            arrow.SetActive(true);
        }
    }
    
    void Update()
    {
        if (canMoveOn & allDead && next.action.WasPressedThisFrame())
        {
            SceneManager.LoadScene(newScene);
            player.transform.position = nextTransformPos;
        }
    }
}
