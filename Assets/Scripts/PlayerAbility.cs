using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : ScriptableObject
{
    internal List<List<Glyph>> grid = new List<List<Glyph>>();
    private void Awake()
    {
        for (int x = 0; x < 7; x++)
        {
            grid.Add(new List<Glyph>());
            for (int y = 0; y < 7; y++)
            {
                Glyph glyph = null;
                if (x == 3 && y == 0 || x == 3 && y == 1 || x == 3 && y == 2 || x == 4 && y == 0 || x == 5 && y == 0)
                {
                    //glyph = ScriptableObject.CreateInstance("Glyph") as Glyph;
                    glyph = Instantiate(GameManager.Instance.testGlyph);
                }
                grid[x].Add(glyph);
            }
        }
    }
}
