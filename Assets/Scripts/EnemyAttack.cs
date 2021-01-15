using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public int damage;
    public int rangeInCells;
    protected Transform playerTransform;
    private void Start()
    {
        playerTransform = GameManager.Instance.player.transform;
    }
    public abstract IEnumerator RequestAction();
    protected virtual bool IsInAttackRange()
    {
        GridManager gridManager = GameManager.Instance.gridManager;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        int distanceInCells = gridManager.GetNrOfCellsBetweenPositions(transform.position, playerPos);
        return distanceInCells <= rangeInCells;
    }
}
