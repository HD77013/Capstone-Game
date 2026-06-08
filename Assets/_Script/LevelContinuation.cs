using System;
using System.Collections;
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

    private Coroutine spawnRoutine;
    public float spawnDelay;
    
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

                if (spawnRoutine != null)
                    StopCoroutine(SpawnEnemies());

                spawnRoutine = StartCoroutine(SpawnEnemies());
            }
            else
            {
                level.onGoingStage = false;
                level.OnAllWavesCleared();
            }

        }
    }

    public IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }

        
        level.FindAllNPCs();
    }
    
    void Update()
    {
        
    }
}
