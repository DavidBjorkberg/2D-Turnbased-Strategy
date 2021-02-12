using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Ability")]
public class PlayerAbility : ScriptableObject
{
    internal List<List<Glyph>> grid = new List<List<Glyph>>();
    internal List<List<Glyph>> rotationGrid = new List<List<Glyph>>();
    [SerializeField] private List<Vector2Int> glyphIndices = new List<Vector2Int>();
    [SerializeField] private List<Glyph> glyphs = new List<Glyph>();

    private void Awake()
    {
        Transform heldGlyphsParent = GameObject.Find("HeldGlyphs").transform;

        for (int x = 0; x < 7; x++)
        {
            grid.Add(new List<Glyph>());
            rotationGrid.Add(new List<Glyph>());
            for (int y = 0; y < 7; y++)
            {
                Glyph glyph = null;
                for (int i = 0; i < glyphIndices.Count; i++)
                {
                    if (glyphIndices[i].x == x && glyphIndices[i].y == y)
                    {
                        glyph = Instantiate(glyphs[i], heldGlyphsParent);
                    }
                }
                grid[x].Add(glyph);
                rotationGrid[x].Add(glyph);
            }
        }
    }
    /// <summary>
    /// Returns the rotated grid based on the mouse pos
    /// </summary>
    /// <returns></returns>
    public List<List<Glyph>> GetRotatedGrid()
    {
        UnRotateRotationGrid();
        return RotateGrid90Degrees(rotationGrid, GetNrOfRotations());
    }
    void UnRotateRotationGrid()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                rotationGrid[i][j] = grid[i][j];
            }
        }
    }
    List<List<Glyph>> RotateGrid90Degrees(List<List<Glyph>> grid, int nrOfRotations)
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
        return grid;
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
}
