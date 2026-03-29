using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboScript : MonoBehaviour
{
    [SerializeField]private PlayerScript player;
    [SerializeField]private Animator animator;
    
    public bool InputBuffer;
    public int comboStep;
    public bool canCombo;
    public int maxCombo = 5;

    public bool OnComboCooldown;
    private float comboResetTime = 5f;
    
    private float comboTimer = 2.0f;
    
    private float comboWindow = 0.5f;
    private void Update()
    {
        
    }

    private void RandomSoundPitching()
    {
        float randomPitch = UnityEngine.Random.Range(0.8f, 1.3f);
        player.soundManager.pitch = randomPitch;
        player.soundManager.PlayOneShot(player.attackSound);
    }
    
    public void StartCombo()
    {
        if (comboStep == 0)
        {
            Debug.Log("Combo started");
            comboStep = 1;
            animator.Play("Attack 1");
            
            player.pRb2d.linearVelocity = Vector2.zero;
            player.pRb2d.linearVelocity = new Vector2(player.GetFacingDirection() * player.forwardForce, 0);

            RandomSoundPitching();
        }

    }
    
    public void ComboStep()
    {
        RandomSoundPitching();
        
        comboStep++;
        canCombo = false;
        InputBuffer = false;
        animator.Play("Attack " + comboStep);
        
        player.pRb2d.linearVelocity = Vector2.zero;
        player.pRb2d.linearVelocity = new Vector2(player.GetFacingDirection() * player.forwardForce, 0);
        
        Debug.Log("Combo step " + comboStep);
        
        if (comboStep >= maxCombo)
        {
            ComboEnd();
        }
    }
    
    // Called by animation events
    public void ComboWindow()
    {
        Debug.Log("Combo window opened"); 
        canCombo = true;
    }
    public void CloseComboWindow()
    {
        Debug.Log("Combo window closed");
        if (InputBuffer && !OnComboCooldown) ComboStep();
        else canCombo = false;    
    }
    
    // Called by event in last attack animation
    public void ComboEnd()
    { 
        comboStep = 0;
        StartCoroutine(ComboCooldown());
    }

    private IEnumerator ComboCooldown()
    {
        OnComboCooldown = true;
        Debug.Log("On cooldown");
        yield return new WaitForSeconds(comboResetTime);
        Debug.Log("Can combo now");
        OnComboCooldown = false;
        
    }
}
