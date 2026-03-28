using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashScript : MonoBehaviour {

    public Color flashColor;
    public float flashDuration;

    Material mat;

    private IEnumerator flashCoroutine;

    private void Awake() {
        mat = GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
    }
    

    public void Flash(){
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
		
        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

    private IEnumerator DoFlash()
    {
        // Flash FX & logic
        
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }
	
    private void SetFlashAmount(float flashAmount)
    {
        // Changes variable of shader
        mat.SetFloat("_FlashAmount", flashAmount);
    }

}