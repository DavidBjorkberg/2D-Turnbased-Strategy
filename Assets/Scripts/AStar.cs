using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public Spot[,] Spots;
    private Vector3 cellSize;
    private int columns;
    private int rows;
    public void Init(int columns, int rows, Vector3 cellSize)
    {
        Spots = new Spot[columns, rows];
        this.cellSize = cellSize;
        this.columns = columns;
        this.rows = rows;
    }
    private bool IsValidPath(Vector3[,] grid, Spot start, Spot end)
    {
        if (end == null)
            return false;
        if (start == null)
            return false;
        if (end.height >= 1)
            return false;
        return true;
    }
    public List<Vector3> CreatePath(Vector3[,] grid, Vector3 start, Vector3 end, int length)
    {
        //if (!IsValidPath(grid, start, end))
        //     return null;

        Spot End = null;
        Spot Start = null;
        var columns = Spots.GetUpperBound(0) + 1;
        var rows = Spots.GetUpperBound(1) + 1;
        Spots = new Spot[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Spots[i, j] = new Spot(grid[i, j].x, grid[i, j].y, grid[i, j].z, columns, rows);
            }
        }

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Spots[i, j].AddNeighbours(Spots, i, j);
                if (IsInsideCell(start, Spots[i, j].GetPos()))
                {
                    Start = Spots[i, j];
                }
                else if (IsInsideCell(end, Spots[i, j].GetPos()))
                {
                    End = Spots[i, j];
                }
            }
        }
        if (!IsValidPath(grid, Start, End))
            return null;
        List<Spot> OpenSet = new List<Spot>();
        List<Spot> ClosedSet = new List<Spot>();

        OpenSet.Add(Start);

        while (OpenSet.Count > 0)
        {
            //Find shortest step distance in the direction of your goal within the open set
            int winner = 0;
            for (int i = 0; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].F < OpenSet[winner].F)
                {
                    winner = i;
                }
                else if (OpenSet[i].F == OpenSet[winner].F)//tie breaking for faster routing
                {
                    if (OpenSet[i].H < OpenSet[winner].H)
                    {
                        winner = i;
                    }
                }
            }

            var current = OpenSet[winner];

            //Found the path, creates and returns the path
            if (End != null && OpenSet[winner] == End)
            {
                List<Vector3> Path = new List<Vector3>();
                var temp = current;
                Path.Add(temp.GetPos());
                while (temp.previous != null)
                {
                    Path.Add(temp.previous.GetPos());
                    temp = temp.previous;
                }
                if (length - (Path.Count - 1) < 0)
                {
                    Path.RemoveRange(0, (Path.Count - 1) - length);
                }
                return Path;
            }

            OpenSet.Remove(current);
            ClosedSet.Add(current);


            //Finds the next closest step on the grid
            var neighboors = current.neighbours;
            for (int i = 0; i < neighboors.Count; i++)//look threw our current spots neighboors (current spot is the shortest F distance in openSet
            {
                var n = neighboors[i];
                if (!ClosedSet.Contains(n) && n.height < 1)//Checks to make sure the neighboor of our current tile is not within closed set, and has a height of less than 1
                {
                    var tempG = current.G + 1;//gets a temp comparison integer for seeing if a route is shorter than our current path

                    bool newPath = false;
                    if (OpenSet.Contains(n)) //Checks if the neighboor we are checking is within the openset
                    {
                        if (tempG < n.G)//The distance to the end goal from this neighboor is shorter so we need a new path
                        {
                            n.G = tempG;
                            newPath = true;
                        }
                    }
                    else//if its not in openSet or closed set, then it IS a new path and we should add it too openset
                    {
                        n.G = tempG;
                        newPath = true;
                        OpenSet.Add(n);
                    }
                    if (newPath)//if it is a newPath caclulate the H and F and set current to the neighboors previous
                    {
                        n.H = Heuristic(n, End);
                        n.F = n.G + n.H;
                        n.previous = current;
                    }
                }
            }

        }
        return null;
    }
    private bool IsInsideCell(Vector3 pos, Vector3 cell)
    {
        float cellMinX = cell.x - cellSize.x / 2;
        float cellMaxX = cell.x + cellSize.x / 2;
        float cellMinY = cell.y - cellSize.y / 2;
        float cellMaxY = cell.y + cellSize.y / 2;

        return pos.x >= cellMinX &&
            pos.x <= cellMaxX &&
            pos.y >= cellMinY &&
            pos.y <= cellMaxY;
    }
    private float Heuristic(Spot a, Spot b)
    {
        //manhattan
        var dx = Math.Abs(a.x - b.x);
        var dy = Math.Abs(a.y - b.y);
        return 1 * (dx + dy);

        //diagonal
        // Chebyshev distance
        //var D = 1;
        //var D2 = 1;
        //octile distance
        //var D = 1;
        //var D2 = 1;
        //var dx = Math.Abs(a.x - b.x);
        //var dy = Math.Abs(a.y - b.y);
        //var result = (int)(1 * (dx + dy) + (D2 - 2 * D));
        //return result;// *= (1 + (1 / 1000));
        //return (int)Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
