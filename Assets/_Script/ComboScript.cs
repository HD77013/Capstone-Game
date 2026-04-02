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
    private float comboResetTime = 1f;
    
    private float comboTimer = 2.0f;
    
    private float comboWindow = 0.5f;

    private float lastAttackTime;
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
        
        if (comboStep >= maxCombo)
        {
            ComboEnd();
        }
        
    }
    
    // Called by animation events
    public void ComboWindow()
    {
        canCombo = true;
    }
    // Called towards the end of the animation
    public void CloseComboWindow()
    {
        if (InputBuffer && !OnComboCooldown) ComboStep();
        else canCombo = false;    
    }
    
    // Called by event in very last keyframe of animation
    public void ComboEnd()
    { 
        comboStep = 0;
        InputBuffer = false;
        animator.SetTrigger("End Combo");
        animator.SetInteger("Combo", 0);
        StartCoroutine(ComboCooldown());
    }

    private IEnumerator ComboCooldown()
    {
        OnComboCooldown = true;
        yield return new WaitForSeconds(comboResetTime);
        
        OnComboCooldown = false;
        
    }
}
