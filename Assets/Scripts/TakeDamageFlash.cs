using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFlash : MonoBehaviour
{
    [SerializeField] private float flashSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Coroutine damageFlashCoroutine;
    private Color baseColor;
    private void Awake()
    {
        baseColor = spriteRenderer.color;
    }
    public void Flash()
    {
        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            spriteRenderer.color = baseColor;
        }
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
