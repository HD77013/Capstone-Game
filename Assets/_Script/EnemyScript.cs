using System;
using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody2D enemyRB;

    public GameObject player;

    public Vector2 direction;

    public bool attackReady;
    
    public LayerMask players;
    
    public GameObject attackPoint;
    public Vector2 boxSize;
    public float castDistance;
    public Vector2 atkBox;
    
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(attackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player.transform.position - transform.position;

        direction = playerPos;
        CheckAttack();
    }

    private void FixedUpdate()
    {
        enemyRB.AddForce(speed * direction);
    }

    bool CheckAttack()
    {
        return Physics2D.BoxCast(attackPoint.transform.position, atkBox, 0, transform.forward, castDistance, players);
    }
    
    

    IEnumerator attackRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("Attack is activated");
        
        if (CheckAttack())
        {
            Debug.Log("Player is damaged");
        }

        StartCoroutine(attackRoutine());
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.transform.position+transform.forward * castDistance, atkBox);
    }
}
