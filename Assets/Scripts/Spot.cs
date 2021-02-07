using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot
{
    public float x;
    public float y;
    public Vector2Int cellIndex;
    public float F;
    public float G;
    public float H;
    public float height = 0;
    public List<Spot> neighbours;
    public Spot previous = null;
    private int gridColumns;
    private int gridRows;
    public Spot(float x, float y, float height, Vector2Int cellIndex, int gridColumns, int gridRows)
    {
        this.x = x;
        this.y = y;
        this.cellIndex = cellIndex;
        this.gridColumns = gridColumns;
        this.gridRows = gridRows;
        F = 0;
        G = 0;
        H = 0;
        neighbours = new List<Spot>();
        this.height = height;
    }
    public Vector3 GetPos()
    {
        return new Vector3(x, y, 0);
    }
    public void AddNeighbours(Spot[,] grid, int x, int y)
    {
        if (x < grid.GetUpperBound(0))
            neighbours.Add(grid[x + 1, y]);
        if (x > 0)
            neighbours.Add(grid[x - 1, y]);
        if (y < grid.GetUpperBound(1))
            neighbours.Add(grid[x, y + 1]);
        if (y > 0)
            neighbours.Add(grid[x, y - 1]);
        #region diagonal 
        if (x > 0 && y > 0)
            neighbours.Add(grid[x - 1, y - 1]);
        if (x < gridColumns - 1 && y > 0)
            neighbours.Add(grid[x + 1, y - 1]);
        if (x > 0 && y < gridRows - 1)
            neighbours.Add(grid[x - 1, y + 1]);
        if (x < gridColumns - 1 && y < gridRows - 1)
            neighbours.Add(grid[x + 1, y + 1]);
        #endregion
    }
}