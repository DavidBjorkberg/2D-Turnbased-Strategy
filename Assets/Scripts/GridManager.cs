using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    internal List<Vector2Int> enemySpawnpointCellIndices = new List<Vector2Int>();
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private AStar aStar;
    [SerializeField] private GameObject walkGrid;
    private Vector3[,] aStarPositions;
    private List<List<GameObject>> cells = new List<List<GameObject>>();
    private GameObject topLeftCell;
    internal BoundsInt gridBounds;

    void Awake()
    {
        Shader.SetGlobalFloat("cellSize", 1);
    }
    public void SwitchRoom(Tilemap walkable, Tilemap collidable, Tilemap bounds)
    {
        bounds.CompressBounds();
        walkable.CompressBounds();
        collidable.CompressBounds();
        gridBounds = bounds.cellBounds;
        CreateGridAndCells(walkable, collidable);
        aStar.Init(gridBounds.size.x, gridBounds.size.y, Vector3.one);
        Shader.SetGlobalVector("topLeftCellPos", cells[0][0].transform.position);
        Shader.SetGlobalFloat("nrOfCellsWithGlyph", 0);
    }
    public void CreateGridAndCells(Tilemap walkable, Tilemap collidable)
    {
        aStarPositions = new Vector3[gridBounds.size.x, gridBounds.size.y];
        DestroyAllCells();
        //TODO: Why is -1 needed on yMax?
        for (int x = gridBounds.xMin, i = 0; i < (gridBounds.size.x); x++, i++)
        {
            cells.Add(new List<GameObject>());
            for (int y = gridBounds.yMax - 1, j = 0; j < (gridBounds.size.y); y--, j++)
            {

                if (collidable.HasTile(new Vector3Int(x, y, 0)))
                {
                    aStarPositions[i, j] = new Vector3(x + 0.5f, y + 0.5f, 1);
                    cells[cells.Count - 1].Add(Instantiate(cellPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, walkGrid.transform));
                    cells[cells.Count - 1][cells[cells.Count - 1].Count - 1].GetComponent<SpriteRenderer>().material = null;

                }
                else if (walkable.HasTile(new Vector3Int(x, y, 0)))
                {
                    aStarPositions[i, j] = new Vector3(x + 0.5f, y + 0.5f, 0);
                    cells[cells.Count - 1].Add(Instantiate(cellPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, walkGrid.transform));
                    if (topLeftCell == null)
                    {
                        topLeftCell = cells[cells.Count - 1][cells[cells.Count - 1].Count - 1];
                    }
                }
            }
        }
    }
    void DestroyAllCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                Destroy(cells[i][j]);
            }
        }
        cells.Clear();

    }

    public Vector2? GetCellPosAtPosition(Vector2 pos)
    {
        for (int i = 0; i < gridBounds.size.x; i++)
        {
            for (int j = 0; j < gridBounds.size.y; j++)
            {
                if (IsInsideCell(pos, cells[i][j].transform.position))
                {
                    if (aStarPositions[i, j].z == 0)
                    {
                        return cells[i][j].transform.position;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }
    public Vector3 GetCellPos(Vector2Int cellIndex)
    {
        return cells[cellIndex.x][cellIndex.y].transform.position;
    }
    private bool IsInsideCell(Vector3 pos, Vector2 cell)
    {
        float cellMinX = cell.x - 0.5f;
        float cellMaxX = cell.x + 0.5f;
        float cellMinY = cell.y - 0.5f;
        float cellMaxY = cell.y + 0.5f;

        return pos.x >= cellMinX &&
            pos.x <= cellMaxX &&
            pos.y >= cellMinY &&
            pos.y <= cellMaxY;
    }
    /// <summary>
    /// Returns a grid containing the nearby indices where 0,0 is top left
    /// </summary>
    /// <returns></returns>
    public List<List<Vector2Int>> GetNearbyCellIndicesAsGrid(Vector3 pos, int range)
    {
        Vector2Int? startIndex = GetCellAtPosition(pos);
        if (startIndex.HasValue)
        {
            List<List<Vector2Int>> returnIndices = new List<List<Vector2Int>>();
            Vector2Int topLeftIndex = startIndex.Value - new Vector2Int(range, range);
            for (int x = 0; x < range * 2 + 1; x++)
            {
                returnIndices.Add(new List<Vector2Int>());
                for (int y = 0; y < range * 2 + 1; y++)
                {
                    returnIndices[x].Add(topLeftIndex + new Vector2Int(x, y));
                }
            }
            return returnIndices;
        }
        else
        {
            print("Couldn't find cell");
            return new List<List<Vector2Int>>();
        }
    }
    public Vector2Int? GetCellAtPosition(Vector3 pos)
    {
        for (int i = 0; i < gridBounds.size.x; i++)
        {
            for (int j = 0; j < gridBounds.size.y; j++)
            {
                if (IsInsideCell(pos, cells[i][j].transform.position))
                {
                    if (aStarPositions[i, j].z == 0)
                    {
                        return new Vector2Int(i, j);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }
    public int GetNrOfCellsBetweenCells(Vector2Int startCellIndex, Vector2Int endCellIndex)
    {
        Vector2Int curCellIndex = startCellIndex;
        int nrOfCells = 0;
        while (curCellIndex != endCellIndex)
        {
            if (curCellIndex.x - endCellIndex.x < 0)
            {
                curCellIndex.x++;
            }
            else if (curCellIndex.x - endCellIndex.x > 0)
            {
                curCellIndex.x--;
            }
            if (curCellIndex.y - endCellIndex.y < 0)
            {
                curCellIndex.y++;
            }
            else if (curCellIndex.y - endCellIndex.y > 0)
            {
                curCellIndex.y--;
            }
            nrOfCells++;
        }
        return nrOfCells;
    }
    public bool IsPathClearBetweenCells(Vector2Int startCellIndex, Vector2Int endCellIndex)
    {
        Vector2Int curCellIndex = startCellIndex;
        bool isPathClear = true;
        while (curCellIndex != endCellIndex && isPathClear)
        {
            if (curCellIndex.x - endCellIndex.x < 0)
            {
                curCellIndex.x++;
            }
            else if (curCellIndex.x - endCellIndex.x > 0)
            {
                curCellIndex.x--;
            }
            if (curCellIndex.y - endCellIndex.y < 0)
            {
                curCellIndex.y++;
            }
            else if (curCellIndex.y - endCellIndex.y > 0)
            {
                curCellIndex.y--;
            }

            if (!IsCellFree(curCellIndex))
            {
                isPathClear = false;
            }
        }
        return isPathClear;
    }
    public bool IsInRange(Vector3 pos1, Vector3 pos2, int rangeInCells)
    {
        Vector2Int? pos1Index = GetCellAtPosition(pos1);
        Vector2Int? pos2Index = GetCellAtPosition(pos2);

        if (pos1Index.HasValue && pos2Index.HasValue)
        {
            bool isXInRange = Mathf.Abs(pos1Index.Value.x - pos2Index.Value.x) <= rangeInCells;
            bool isYInRange = Mathf.Abs(pos1Index.Value.y - pos2Index.Value.y) <= rangeInCells;

            return isXInRange && isYInRange;
        }
        else
        {
            print("Tried to get cell outside of grid");
            return false;
        }
    }
    public bool IsInRange(Vector2 pos1Index, Vector3 pos2, int rangeInCells)
    {
        Vector2Int? pos2Index = GetCellAtPosition(pos2);

        if (pos2Index.HasValue)
        {
            bool isXInRange = Mathf.Abs(pos1Index.x - pos2Index.Value.x) <= rangeInCells;
            bool isYInRange = Mathf.Abs(pos1Index.y - pos2Index.Value.y) <= rangeInCells;

            return isXInRange && isYInRange;
        }
        else
        {
            print("Tried to get cell outside of grid");
            return false;
        }
    }
    public Vector2Int GetGridSize()
    {
        return new Vector2Int(gridBounds.size.x, gridBounds.size.y);
    }
    public bool IsCellFree(Vector2Int index)
    {
        return aStarPositions[index.x, index.y].z == 0;
    }
    public GameObject GetCell(Vector2Int index)
    {
        return cells[index.x][index.y];
    }
    public List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        return aStar.CreatePath(aStarPositions, start, end, 1000);
    }
}
