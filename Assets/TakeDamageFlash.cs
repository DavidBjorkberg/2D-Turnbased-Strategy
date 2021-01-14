using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFlash : MonoBehaviour
{
    public float flashSpeed;
    public SpriteRenderer spriteRenderer;
    private Coroutine damageFlashCoroutine;
    public void Flash()
    {
        damageFlashCoroutine = StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        float lerpValue = 0;
        Color startColour = spriteRenderer.color;
        Color targetColour = Color.red;
        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * flashSpeed;
            spriteRenderer.color = Color.Lerp(startColour, targetColour, lerpValue);
            yield return new WaitForEndOfFrame();
        }

        targetColour = startColour;
        startColour = spriteRenderer.color;
        lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * flashSpeed;
            spriteRenderer.color = Color.Lerp(startColour, targetColour, lerpValue);
            yield return new WaitForEndOfFrame();
        }

    }
}
