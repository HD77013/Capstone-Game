using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D pRb2d;
    
    public InputActionReference movement;
    public InputActionReference jumping;
    public InputActionReference attack;
    
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    
    public Vector2 direction;
    
    public float speed;
    public float jump;

    public GameObject attackPoint;
    public Vector2 atkBox;
    public float atkCastDis;
    public LayerMask enemies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        direction = movement.action.ReadValue<Vector2>();

        if (jumping.action.WasPressedThisFrame() && Grounded())
        {
            Debug.Log("Jumping");
            pRb2d.AddForce(new Vector2(pRb2d.linearVelocity.x, jump));
        }


        if (attack.action.WasPressedThisFrame() && Grounded())
        {
            Attack();
        }
        
    }

    private void FixedUpdate()
    {
        pRb2d.AddForce(direction * speed);
    }

    public bool Grounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
    }
    
    // Will be actived by anim event (for now, the moment input is pressed)
    void Attack()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(attackPoint.transform.position, atkBox, 0, enemies);

        foreach (Collider2D enemyGameObject in enemy)
        {
            Debug.Log("Enemy is hit!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
        
        Gizmos.DrawWireCube(attackPoint.transform.position, atkBox);
    }
}
