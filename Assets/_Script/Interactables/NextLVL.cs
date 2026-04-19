using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLVL : MonoBehaviour
{
    [SerializeField]private bool canMoveOn;
    private PlayerScript player;

    [SerializeField]private InputActionReference next;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        
        if (player != null)
        {
            canMoveOn = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player != null)
        {
            canMoveOn = false;
        }
    }


    void Start()
    {

    }
    
    void Update()
    {
        if (canMoveOn && next.action.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Next");
        }
    }
}
