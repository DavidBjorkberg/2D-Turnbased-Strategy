using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : EnemyMovement
{
    RangedEnemyCombat combat;

    private void Awake()
    {
        combat = GetComponent<RangedEnemyCombat>();
    }
    protected override bool ShouldPath()
    {
        return !IsInAttackRange() && base.ShouldPath();
    }
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= combat.range;
    }
}
