using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttackGrid : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private GameObject cellPrefab;
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        for (int i = -gridWidth / 2; i < gridWidth / 2; i++)
        {
            for (int j = -gridHeight /  2; j < gridHeight / 2; j++)
            {
                Vector3 spawnPos = playerPos + new Vector3(i, j);
                Instantiate(cellPrefab, spawnPos, Quaternion.identity, transform);
            }
        }
    }

}
