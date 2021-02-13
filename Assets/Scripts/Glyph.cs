using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Glyph : MonoBehaviour
{
    public Sprite sprite;
    internal Vector2Int cellIndex;
    internal bool isProcessing = false;
    public virtual void Process(bool endOfTurn)
    {
        isProcessing = true;
        CheckCondition();
    }
    protected virtual void CheckCondition()
    {
        List<Enemy> allEnemies = GameManager.Instance.enemyManager.GetAllEnemies();
        bool isTriggered = false;
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (GameManager.Instance.gridManager.IsInRange(cellIndex, allEnemies[i].GetCurrentCellIndex(), 0))
            {
                Trigger(allEnemies[i]);
                isTriggered = true;
            }
        }
        if(!isTriggered)
        {
            isProcessing = false;
        }

    }
    public virtual void UpdateGlyph()
    {

    }
    protected abstract void Trigger(Enemy enemy);
}