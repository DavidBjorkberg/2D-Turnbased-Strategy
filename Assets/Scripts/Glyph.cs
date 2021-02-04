using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Glyph/Glyph")]

public class Glyph : ScriptableObject
{
    public Sprite sprite;
    public Vector2Int cellIndex;

    public void Process()
    {
        CheckCondition();
    }
    void CheckCondition()
    {

        if (GameManager.Instance.gridManager.IsInRange(cellIndex, GameManager.Instance.roundManager.enemyTest.transform.position, 0))
        {
            Trigger();
        }
    }
    void Trigger()
    {
        Debug.Log("TRIGGERED");
        GameManager.Instance.glyphManager.RemoveGlyph(cellIndex);
    }
}
