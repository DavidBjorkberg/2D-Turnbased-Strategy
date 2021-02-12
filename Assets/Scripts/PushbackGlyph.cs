using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushbackGlyph : Glyph
{
    [SerializeField] private int pushbackDistance;
    private Vector2Int pushbackDir = Vector2Int.up;
    protected override void Trigger(Enemy enemy)
    {

    }

    IEnumerator Pushback(Enemy enemy)
    {
        if (GameManager.Instance.IsCellFree(cellIndex + pushbackDir))
        {
            Vector3 startPos = enemy.transform.position;
            Vector3 endPos = GameManager.Instance.gridManager.GetCellPos(cellIndex + pushbackDir);
            float lerpValue = 0;

            while (lerpValue < 1)
            {
                lerpValue += Time.deltaTime * movementSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, lerpValue);
                yield return new WaitForEndOfFrame();
            }
            GetComponent<Enemy>().SetCurrentCellIndex(GameManager.Instance.gridManager.GetCellAtPosition(endPos).Value);
            path.RemoveAt(path.Count - 1);

        }
    }

}
