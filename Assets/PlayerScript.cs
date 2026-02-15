using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D pRb2d;
    public InputActionReference movement;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    
    public Vector2 direction;
    
    
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = movement.action.ReadValue<Vector2>();

        if (Grounded())
        {
            Debug.Log("Player is grounded");
            
        }
        else
        {
            Debug.Log("Player is in air");
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
