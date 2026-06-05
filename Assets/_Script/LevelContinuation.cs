using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelContination : MonoBehaviour
{
    [SerializeField]private bool allDead;
    [SerializeField]private bool canMoveOn;
    private PlayerScript player;

    public List<EnemyScript> npcs = new List<EnemyScript>();

    void Start()
    {
        FindAllNPCs();
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
        }
    }
    
    void Update()
    {
        
    }
}
