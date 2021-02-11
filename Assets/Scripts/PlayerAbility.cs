using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Ability")]
public class PlayerAbility : ScriptableObject
{
    internal List<List<Glyph>> grid = new List<List<Glyph>>();
    [SerializeField] private List<Vector2Int> glyphIndices = new List<Vector2Int>();
    [SerializeField] private List<Glyph> glyphs = new List<Glyph>();
    public void Init()
    {
        for (int x = 0; x < 7; x++)
        {
            grid.Add(new List<Glyph>());
            for (int y = 0; y < 7; y++)
            {
                Glyph glyph = null;
                for (int i = 0; i < glyphIndices.Count; i++)
                {
                    if (glyphIndices[i].x == x && glyphIndices[i].y == y)
                    {
                        glyph = Instantiate(glyphs[i]);
                    }
                }
                grid[x].Add(glyph);

                //if (x == 3 && y == 0 || x == 3 && y == 1 || x == 3 && y == 2 || x == 4 && y == 0 || x == 5 && y == 0)
                //{
                //    glyph = Instantiate(GameManager.Instance.testGlyph);
                //}
            }
        }
    }
}
