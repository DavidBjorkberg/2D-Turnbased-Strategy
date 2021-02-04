using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap walkable;
    public Tilemap collidable;
    public GameObject cellPrefab;
    public Grid grid;
    public Vector3[,] aStarPositions;
    public AStar aStar;
    [SerializeField] private GameObject walkGrid;
    private List<List<GameObject>> cells = new List<List<GameObject>>();
    private GameObject topLeftCell;
    BoundsInt bounds;

    void Awake()
    {
        walkable.CompressBounds();
        collidable.CompressBounds();
        if (walkable.cellBounds != collidable.cellBounds)
        {
            Debug.LogError("Walkable cellbounds are not identical to collidable cellbounds");
        }
        Shader.SetGlobalFloat("cellSize", grid.cellSize.x);
        bounds = walkable.cellBounds;
        CreateGridAndCells();
        Shader.SetGlobalVector("topLeftCellPos", cells[0][0].transform.position);
        Shader.SetGlobalFloat("nrOfCellsWithGlyph", 0);
        aStar.Init(bounds.size.x, bounds.size.y, grid.cellSize);
    }
    public void CreateGridAndCells()
    {
        aStarPositions = new Vector3[bounds.size.x, bounds.size.y];
        //TODO: Why is -1 needed on yMax?
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            cells.Add(new List<GameObject>());
            for (int y = bounds.yMax - 1, j = 0; j < (bounds.size.y); y--, j++)
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
                    if(topLeftCell == null)
                    {
                        topLeftCell = cells[cells.Count - 1][cells[cells.Count - 1].Count - 1];
                    }
                }
            }
        }
    }

    public Vector2? GetCellPosAtPosition(Vector2 pos)
    {
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
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
    private bool IsInsideCell(Vector3 pos, Vector2 cell)
    {
        float cellMinX = cell.x - grid.cellSize.x / 2;
        float cellMaxX = cell.x + grid.cellSize.x / 2;
        float cellMinY = cell.y - grid.cellSize.y / 2;
        float cellMaxY = cell.y + grid.cellSize.y / 2;

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
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
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
    public int GetNrOfCellsBetweenPositions(Vector3 pos1, Vector3 pos2)
    {
        return aStar.CreatePath(aStarPositions, pos1, pos2, 1000).Count - 1;
    }
    public bool IsInNeighbourCell(Vector3 pos1, Vector3 pos2)
    {
        float distance = Vector2.Distance(pos1, pos2);
        float pow = Mathf.Pow(grid.cellSize.x, 2) + Mathf.Pow(grid.cellSize.x, 2);
        float diagonalDistance = Mathf.Sqrt(pow);
        return distance <= diagonalDistance;
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
