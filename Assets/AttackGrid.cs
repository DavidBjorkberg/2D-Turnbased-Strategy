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
    private List<List<Glyph>> curAbilityGrid;
    void Start()
    {
        Spawn();
    }
    private void Update()
    {
        if (isActive)
        {
            List<List<Glyph>> rotationGrid = GameManager.Instance.DeepCopyGrid(curAbilityGrid);
            RotateGrid90Degrees(ref rotationGrid, GetNrOfRotations());
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
    public void ActivateGrid(List<List<Glyph>> abilityGrid)
    {
        isActive = true;
        curAbilityGrid = abilityGrid;
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
    int GetNrOfRotations()
    {
        Vector2 mousePos = GameManager.Instance.GetMousePosInWorld();
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        Vector2 playerToMouseDir = (mousePos - playerPos).normalized;

        if (playerToMouseDir.x > 0)
        {
            if (playerToMouseDir.y > 0)
            {
                if (playerToMouseDir.x > playerToMouseDir.y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (playerToMouseDir.x > Mathf.Abs(playerToMouseDir.y))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        else
        {
            if (playerToMouseDir.y > 0)
            {
                if (Mathf.Abs(playerToMouseDir.x) > playerToMouseDir.y)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (Mathf.Abs(playerToMouseDir.x) > Mathf.Abs(playerToMouseDir.y))
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
            }
        }
    }
    void RotateGrid90Degrees(ref List<List<Glyph>> grid, int nrOfRotations)
    {
        for (int i = 0; i < nrOfRotations; i++)
        {
            grid = TransposeGrid(grid);
            for (int y = 0; y < grid.Count; y++)
            {
                for (int x = 0; x < Mathf.FloorToInt(grid[y].Count / 2); x++)
                {
                    Glyph temp = grid[x][y];
                    int rightSideSwitchIndex = grid[y].Count - x - 1;

                    grid[x][y] = grid[rightSideSwitchIndex][y];
                    grid[rightSideSwitchIndex][y] = temp;
                }
            }
        }
    }
    List<List<Glyph>> TransposeGrid(List<List<Glyph>> grid)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = i; j < grid[i].Count; j++)
            {
                Glyph temp = grid[i][j];

                grid[i][j] = grid[j][i];
                grid[j][i] = temp;
            }
        }
        return grid;
    }
    public List<List<Glyph>> GetRotatedGrid()
    {
        List<List<Glyph>> rotationGrid = GameManager.Instance.DeepCopyGrid(curAbilityGrid);
        RotateGrid90Degrees(ref rotationGrid, GetNrOfRotations());
        return rotationGrid;
    }

}
