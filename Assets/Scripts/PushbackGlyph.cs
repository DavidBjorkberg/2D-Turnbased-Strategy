using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushbackGlyph : Glyph
{
    [SerializeField] private int pushbackDistance;
    [SerializeField] private float pushbackSpeed;
    private Vector2Int pushbackDir = -Vector2Int.up;
    private Enemy hitEnemy;
    private float lerpValue = 0;
    private Vector3 startPos;
    private Vector3 endPos;
    protected override void Trigger(Enemy enemy)
    {
        if (GameManager.Instance.IsCellFree(cellIndex + pushbackDir))
        {
            hitEnemy = enemy;
            startPos = hitEnemy.transform.position;
            endPos = GameManager.Instance.gridManager.GetCellPos(cellIndex + pushbackDir);
        }
        else
        {
            GameManager.Instance.glyphManager.RemoveGlyph(cellIndex);
        }
    }
    public override void UpdateGlyph()
    {
        lerpValue += Time.deltaTime * pushbackSpeed;
        hitEnemy.transform.position = Vector3.Lerp(startPos, endPos, lerpValue);

        if (lerpValue >= 1)
        {
            hitEnemy.SetCurrentCellIndex(GameManager.Instance.gridManager.GetCellAtPosition(endPos).Value);
            hitEnemy.SetClaimedCellIndex(GameManager.Instance.gridManager.GetCellAtPosition(endPos).Value);
            GameManager.Instance.glyphManager.RemoveGlyph(cellIndex);
            GameManager.Instance.glyphManager.processGlyphsData.didGlyphMoveEnemy = true;
        }

    }

}
