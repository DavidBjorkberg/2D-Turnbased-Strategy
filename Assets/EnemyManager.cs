using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    private Tilemap enemySpawnpoints;
    private List<Vector2Int> spawnpointCellIndices = new List<Vector2Int>();
    private List<Enemy> enemies = new List<Enemy>();

    public void SwitchRoom(Tilemap enemySpawnpoints)
    {
        DestroyAllEnemies();
        this.enemySpawnpoints = enemySpawnpoints;
        ReadInSpawnpoints();
        SpawnEnemies();
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
    void EnemyDied(int index)
    {
        Destroy(enemies[index].gameObject);
        enemies.RemoveAt(index);
        if (enemies.Count <= 0)
        {
            GameManager.Instance.roomManager.LoadNextRoom();
        }
    }
    public void ProcessDeadEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (!enemies[i].enemyHealth.IsAlive())
            {
                EnemyDied(i);
            }
        }
    }
    void DestroyAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }
        enemies.Clear();
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
    void ReadInSpawnpoints()
    {
        BoundsInt bounds = GameManager.Instance.gridManager.gridBounds;
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMax - 1, j = 0; j < (bounds.size.y); y--, j++)
            {
                if (enemySpawnpoints.HasTile(new Vector3Int(x, y, 0)))
                {
                    spawnpointCellIndices.Add(new Vector2Int(i, j));
                }
            }
        }
    }
    void SpawnEnemies()
    {
        for (int i = 0; i < spawnpointCellIndices.Count; i++)
        {
            Vector3 spawnPos = GameManager.Instance.gridManager.GetCellPos(spawnpointCellIndices[i]);
            Enemy instantiatedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            instantiatedEnemy.SetCurrentCellIndex(spawnpointCellIndices[i]);
            instantiatedEnemy.SetClaimedCellIndex(spawnpointCellIndices[i]);
            enemies.Add(instantiatedEnemy);
        }
    }
}
