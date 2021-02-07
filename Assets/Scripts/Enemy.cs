using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal EnemyHealth enemyHealth;
    internal EnemyAttack enemyAttack;
    internal EnemyMovement enemyMovement;
    internal EnemyActionPicker enemyActionPicker;
    private Vector2Int curCellIndex;
    private Vector2Int claimedCellIndex;
    
    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyActionPicker = GetComponent<EnemyActionPicker>();
    }
    public Vector2Int GetCurrentCellIndex()
    {
        return curCellIndex;
    }
    public void SetCurrentCellIndex(Vector2Int cellIndex)
    {
        curCellIndex = cellIndex;
    }
    public Vector2Int GetClaimedCellIndex()
    {
        return claimedCellIndex;
    }
    public void SetClaimedCellIndex(Vector2Int cellIndex)
    {
        claimedCellIndex = cellIndex;
    }
}
