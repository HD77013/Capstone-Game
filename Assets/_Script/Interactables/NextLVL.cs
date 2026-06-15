using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLVL : MonoBehaviour
{
    public LevelContination level;
    
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
    
    public bool onGoingStage;

    public string prompt;
    

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

            if (allDead && !onGoingStage)
            {
                text.SetActive(true);
                textMesh.text = prompt;
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

        foreach (EnemyScript npc in found)
        {
            if (!npc.isDead)
                npcs.Add(npc);
        }
    }

    public void RequestRemove(EnemyScript npc)
    {
        npcs.Remove(npc);
        
        // If a scene allows waves of enemies (level exists in scene), OnAllWavesCleared() will be called instead
        if (npcs.Count == 0 && level == null)
        {
            allDead = true;
            arrow.SetActive(true);
        }

        if (level != null)
            level.CheckForNPC(npc);
        
    }
    
    public void OnAllWavesCleared()
    {
        allDead = true;
        arrow.SetActive(true);
    }
    
    void Update()
    {
        if (canMoveOn & allDead && next.action.WasPressedThisFrame() && !onGoingStage)
        {
            SceneManager.LoadScene(newScene);
            player.transform.position = nextTransformPos;
        }
    }
}
