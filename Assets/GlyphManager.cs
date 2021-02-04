using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    List<Vector4> cellsWithGlyph = new List<Vector4>();
    int nrOfCellsWithGlyph = 0;
    private void Awake()
    {
        for (int i = 0; i < 1000; i++)
        {
            cellsWithGlyph.Add(new Vector4());
        }
    }
    public void PlaceGlyph(Glyph glyph, Vector2Int cellIndex)
    {
        GameObject cell = GameManager.Instance.gridManager.GetCell(cellIndex);
        cell.GetComponent<SpriteRenderer>().sprite = glyph.sprite;
        cellsWithGlyph[nrOfCellsWithGlyph] = new Vector4(cellIndex.x, cellIndex.y, 0, 0);
        nrOfCellsWithGlyph++;
        Shader.SetGlobalVectorArray("cellsWithGlyph", cellsWithGlyph);
        Shader.SetGlobalFloat("nrOfCellsWithGlyph", nrOfCellsWithGlyph);
    }
}