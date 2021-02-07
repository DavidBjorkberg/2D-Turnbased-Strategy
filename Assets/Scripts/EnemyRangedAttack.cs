using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttack
{
    public Fireball fireball;
    public override IEnumerator RequestAction()
    {
        if (IsInAttackRange() && IsAttackPathClear())
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
        float lerpValue = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = playerTransform.position;
        Fireball instantiatedFireball = Instantiate(fireball, transform.position, Quaternion.identity);
        instantiatedFireball.transform.right = (endPos - startPos).normalized;

        while (lerpValue < 1 && instantiatedFireball != null)
        {
            lerpValue += Time.deltaTime * instantiatedFireball.speed;
            instantiatedFireball.transform.position = Vector3.Lerp(startPos, endPos, lerpValue);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
