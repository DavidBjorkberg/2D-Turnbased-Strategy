using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    public override IEnumerator RequestAction()
    {
        if (IsInAttackRange())
        {
            return Attack();
        }
        else
        {
            return null;
        }
    }
    IEnumerator Attack()
    {
        SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        float lerpValue = 0;
        Color startColour = spriteRenderer.color;
        Color targetColour = Color.red;
        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * 5;
            spriteRenderer.color = Color.Lerp(startColour, targetColour, lerpValue);
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.player.playerHealth.TakeDamage(damage);
        targetColour = startColour;
        startColour = spriteRenderer.color;
        lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * 5;
            spriteRenderer.color = Color.Lerp(startColour, targetColour, lerpValue);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

}
