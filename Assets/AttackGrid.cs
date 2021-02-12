using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGrid : MonoBehaviour
{
    internal List<List<GameObject>> visualGrid = new List<List<GameObject>>();
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private GameObject cellPrefab;
    private bool isActive;
    private PlayerAbility curAbility;
    private void Awake()
    {
        List<Vector4> cellsToRender = new List<Vector4>();
        for (int i = 0; i < 7 * 7; i++)
        {
            cellsToRender.Add(-Vector4.one);
        }
        Shader.SetGlobalVectorArray("cellsToRender", cellsToRender);

    }
    void Start()
    {
        Spawn();
    }
    private void Update()
    {
        if (isActive)
        {
            List<List<Glyph>> rotationGrid = curAbility.GetRotatedGrid();
            List<Vector4> cellsToRender = new List<Vector4>();
            for (int y = 0; y < rotationGrid.Count; y++)
            {
                for (int x = 0; x < rotationGrid[y].Count; x++)
                {
                    if (rotationGrid[x][y])
                    {
                        cellsToRender.Add(new Vector4(x, y, 0, 0));
                        visualGrid[x][y].GetComponent<SpriteRenderer>().sprite = rotationGrid[x][y].sprite;
                    }
                    visualGrid[x][y].SetActive(true);
                }
            }
            Shader.SetGlobalVectorArray("cellsToRender", cellsToRender);
        }
    }
    void Spawn()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        for (int i = -gridWidth / 2; i <= gridWidth / 2; i++)
        {
            visualGrid.Add(new List<GameObject>());
            for (int j = gridHeight / 2; j >= -gridHeight / 2; j--)
            {
                Vector3 spawnPos = playerPos + new Vector3(i, j);
                visualGrid[visualGrid.Count - 1].Add(Instantiate(cellPrefab, spawnPos, Quaternion.identity, transform));
            }
        }
        DeactivateGrid();
    }
    public void ActivateGrid(PlayerAbility ability)
    {
        isActive = true;
        curAbility = ability;
        Shader.SetGlobalFloat("playerTakingWalkInput", 0);

    }
    public void DeactivateGrid()
    {
        isActive = false;
        for (int i = 0; i < visualGrid.Count; i++)
        {
            for (int j = 0; j < visualGrid[i].Count; j++)
            {
                visualGrid[i][j].SetActive(false);
            }
        }
        Shader.SetGlobalFloat("playerTakingWalkInput", 1);

    }
    public bool IsGridActive()
    {
        return isActive;
    }
}
