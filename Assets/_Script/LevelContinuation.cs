using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelContination : MonoBehaviour
{
    public NextLVL level;
    private PlayerScript player;

    public int spawnAmount;

    public int waves;
    private bool clearedWaves;
    
    [Tooltip("Enemy prefab here")]
    public GameObject enemy;

    void Start()
    {
        
    }

    public void CheckForNPC(EnemyScript npc)
    {
        if (level.npcs.Count == 0)
        {
            Debug.Log("Checking Wave");

            if (waves != 0)
            {
                waves--;
                Invoke(nameof(SpawnEnemies), 3.5f);
            }
            else
            {
                level.onGoingStage = false;
                level.OnAllWavesCleared();
            }

        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < spawnAmount; i++)
            Instantiate(enemy, transform.position, Quaternion.identity);
        
        level.FindAllNPCs();
    }
    
    void Update()
    {
        
    }
}
