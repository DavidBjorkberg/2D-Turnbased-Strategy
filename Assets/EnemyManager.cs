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
            enemies.Add(Instantiate(enemyPrefab, spawnPos, Quaternion.identity));
        }

    }

    public IEnumerator ProcessEnemies()
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        for (int i = 0; i < enemies.Count; i++)
        {
            coroutines.Add(StartCoroutine(enemies[i].GetComponent<EnemyActionPicker>().RequestAction()));
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
            if (enemies[i].GetCurrentCellIndex() == cellIndex)
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
