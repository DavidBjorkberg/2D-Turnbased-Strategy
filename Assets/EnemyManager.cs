using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    private List<Enemy> enemies = new List<Enemy>();
    private List<Vector2Int> spawnPointCellIndices = new List<Vector2Int>();
    void Start()
    {
        spawnPointCellIndices = GameManager.Instance.gridManager.spawnPointCellIndices;
        for (int i = 0; i < spawnPointCellIndices.Count; i++)
        {
            Vector3 spawnPos = GameManager.Instance.gridManager.GetCellPos(spawnPointCellIndices[i]);
            Enemy instantiatedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            instantiatedEnemy.SetCurrentCellIndex(spawnPointCellIndices[i]);
            instantiatedEnemy.SetClaimedCellIndex(spawnPointCellIndices[i]);
            enemies.Add(instantiatedEnemy);
        }
    }

    public IEnumerator ProcessEnemies()
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].enemyHealth.IsAlive())
            {
                coroutines.Add(StartCoroutine(enemies[i].GetComponent<EnemyActionPicker>().RequestAction()));
            }
            else
            {
                enemies.RemoveAt(i);
            }
        }
        for (int i = 0; i < coroutines.Count; i++)
        {
            yield return coroutines[i];
        }
        GameManager.Instance.roundManager.EndEnemyTurn();

    }
    public bool IsCellFreeFromEnemies(Vector2Int cellIndex)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetCurrentCellIndex() == cellIndex || enemies[i].GetClaimedCellIndex() == cellIndex)
            {
                return false;
            }
        }
        return true;
    }
    public List<Enemy> GetAllEnemies()
    {
        return enemies;
    }
}
