using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public PlayerStateManager player;
    public CameraScript camera;

    public GameObject UI;
    
    public Animator animator;
    public Rigidbody2D pRb2d;
    
    public CanvasGroup deathScreen;
    public AudioSource source;
    
    public AudioClip screenSound;
    public AudioClip deathSound;
    
    public void OnDeath(Transform knockbackSource , float knockbackForce, float duration)
    {
        Debug.Log("Player is dead");
        
        Vector2 dir = ((Vector2)transform.position
                       - (Vector2)knockbackSource.position).normalized;

        pRb2d.linearVelocity = Vector2.zero;
        pRb2d.linearVelocity = new Vector2(dir.x * knockbackForce, 0f);

        camera.zoomVal = 3.0f;
        camera.zoom = true;
        
        animator.Play("Death");
        source.PlayOneShot(deathSound);
        
        Invoke("PrepareDeathScreen", 5.0f);
    }

    void PrepareDeathScreen()
    {
        source.PlayOneShot(screenSound);

        Invoke("DeathScreen", 1.0f);
    }

    void DeathScreen()
    {
        deathScreen.alpha = 1f;
        
        Invoke("Respawn", 5.0f);
    }

    void Respawn()
    {
        camera.zoom = false;
        deathScreen.alpha = 0f;

        string firstLVL = SceneManager.GetActiveScene().name;
    
        Destroy(player.gameObject);
        Destroy(camera.gameObject);
        Destroy(UI);
    
        SceneManager.LoadScene(firstLVL);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
