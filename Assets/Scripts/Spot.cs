using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot
{
    public float x;
    public float y;
    public float F;
    public float G;
    public float H;
    public float height = 0;
    public List<Spot> neighbours;
    public Spot previous = null;
    public Spot(float x, float y, float height)
    {
        this.x = x;
        this.y = y;
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
        //if (X > 0 && Y > 0)
        //    Neighboors.Add(grid[X - 1, Y - 1]);
        //if (X < Utils.Columns - 1 && Y > 0)
        //    Neighboors.Add(grid[X + 1, Y - 1]);
        //if (X > 0 && Y < Utils.Rows - 1)
        //    Neighboors.Add(grid[X - 1, Y + 1]);
        //if (X < Utils.Columns - 1 && Y < Utils.Rows - 1)
        //    Neighboors.Add(grid[X + 1, Y + 1]);
        #endregion
    }
}