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

        return gridManager.IsInRange(playerPos, transform.position, rangeInCells);
    }
    protected virtual bool IsAttackPathClear()
    {
        GridManager gridManager = GameManager.Instance.gridManager;
        Vector2Int playerCellIndex = GameManager.Instance.player.playerMovement.GetCurrentCellIndex();
        Vector2Int curCellIndex = GetComponent<Enemy>().GetCurrentCellIndex();
        bool isPathClearToPlayer = gridManager.IsPathClearBetweenCells(curCellIndex, playerCellIndex);

        return isPathClearToPlayer;
    }
}
