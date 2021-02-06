using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal EnemyHealth enemyHealth;
    internal EnemyAttack enemyAttack;
    internal EnemyMovement enemyMovement;
    internal EnemyActionPicker enemyActionPicker;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyActionPicker = GetComponent<EnemyActionPicker>();
    }
    public Vector2Int GetCurrentCellIndex()
    {
        return GameManager.Instance.gridManager.GetCellAtPosition(transform.position).Value;
    }
}
