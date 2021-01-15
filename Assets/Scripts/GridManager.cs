using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap walkable;
    public Tilemap collidable;
    public Grid grid;
    public Vector3[,] spots;
    public AStar aStar;
    BoundsInt bounds;

    List<Spot> path = new List<Spot>();
    void Awake()
    {
        walkable.CompressBounds();
        collidable.CompressBounds();
        if (walkable.cellBounds != collidable.cellBounds)
        {
            Debug.LogError("Walkable cellbounds are not identical to collidable cellbounds");
        }
        bounds = walkable.cellBounds;
        CreateGrid();
        aStar.Init(bounds.size.x, bounds.size.y, grid.cellSize);
    }
    public void CreateGrid()
    {
        spots = new Vector3[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (collidable.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3(x + 0.5f, y + 0.5f, 1);
                }
                else if (walkable.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3(x + 0.5f, y + 0.5f, 0);
                }
            }
        }
    }
    public Vector3? GetCellPosAtPosition(Vector3 pos)
    {
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
                if (IsInsideCell(pos, spots[i, j]))
                {
                    return spots[i, j];
                }
            }
        }
        return null;
    }
    private bool IsInsideCell(Vector3 pos, Vector3 cell)
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
    public bool IsInNeighbourCell(Vector3 pos1, Vector3 pos2)
    {
        float distance = Vector2.Distance(pos1, pos2);

        return distance <= 1.4f;
    }
    public List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        return aStar.CreatePath(spots, start, end, 1000);
    }
}