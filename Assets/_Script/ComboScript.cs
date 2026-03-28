using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboScript : MonoBehaviour
{
    [SerializeField]private PlayerScript player;
    [SerializeField]private Animator animator;
    
    public bool InputBuffer;
    public int comboStep;
    public bool canCombo;
    private int maxCombo = 5;
    
    private float comboTimer = 2.0f;
    [SerializeField]private float lastAttackTime;
    
    private float comboWindow = 1.5f;
    private void Update()
    {
        if (Time.time - lastAttackTime <= comboWindow)
            ComboWindow();
        if (Time.time - lastAttackTime >= comboWindow && canCombo)
            CloseComboWindow();
        
        if (comboStep > 0 && Time.time - lastAttackTime >= comboTimer)
            ComboEnd();
        
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
            lastAttackTime = Time.time;
            comboStep = 1;
            player.Attack();
//            animator.SetTrigger("Attack");

            RandomSoundPitching();
        }

    }
    
    public void ComboStep()
    {
        RandomSoundPitching();
        
        lastAttackTime = Time.time;
        comboStep++;
        canCombo = false;
        InputBuffer = false;
        player.Attack();
      //  animator.SetTrigger("Attack" + comboStep);
        
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
        if (InputBuffer) ComboStep();
        else canCombo = false;    
    }
    
    // Called by event in last attack animation
    public void ComboEnd() => comboStep = 0;
}
