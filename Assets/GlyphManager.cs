using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    [SerializeField] private Transform placedGlyphsParent;
    List<Vector4> cellsWithGlyph = new List<Vector4>();
    List<Glyph> placedGlyphs = new List<Glyph>();
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

        Glyph glyphCopy = Instantiate(glyph, placedGlyphsParent);
        glyphCopy.cellIndex = cellIndex;
        placedGlyphs.Add(glyphCopy);

        cellsWithGlyph[nrOfCellsWithGlyph] = new Vector4(cellIndex.x, cellIndex.y, 0, 0);
        nrOfCellsWithGlyph++;

        Shader.SetGlobalVectorArray("cellsWithGlyph", cellsWithGlyph);
        Shader.SetGlobalFloat("nrOfCellsWithGlyph", nrOfCellsWithGlyph);
    }
    public void ProcessGlyphs(bool endOfTurn)
    {
        for (int i = 0; i < placedGlyphs.Count; i++)
        {
            placedGlyphs[i].Process(endOfTurn);
        }
    }
    public void RemoveGlyph(Vector2Int cellIndexOfGlyph)
    {
        for (int i = 0; i < cellsWithGlyph.Count; i++)
        {
            if (cellsWithGlyph[i].x == cellIndexOfGlyph.x && cellsWithGlyph[i].y == cellIndexOfGlyph.y)
            {
                cellsWithGlyph.RemoveAt(i);
                break;
            }
        }
        nrOfCellsWithGlyph--;

        for (int i = 0; i < placedGlyphs.Count; i++)
        {
            if (placedGlyphs[i].cellIndex == cellIndexOfGlyph)
            {
                Destroy(placedGlyphs[i].gameObject);
                placedGlyphs.RemoveAt(i);
            }
        }

        Shader.SetGlobalVectorArray("cellsWithGlyph", cellsWithGlyph);
        Shader.SetGlobalFloat("nrOfCellsWithGlyph", nrOfCellsWithGlyph);
    }
}
