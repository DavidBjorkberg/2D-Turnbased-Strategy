using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Glyph : MonoBehaviour
{
    public Sprite sprite;
    internal Vector2Int cellIndex;
    public virtual void Process(bool endOfTurn)
    {
        CheckCondition();
    }
    protected virtual void CheckCondition()
    {
        List<Enemy> allEnemies = GameManager.Instance.enemyManager.GetAllEnemies();

        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (GameManager.Instance.gridManager.IsInRange(cellIndex, allEnemies[i].transform.position, 0))
            {
                Trigger(allEnemies[i]);
            }
        }
    }
    protected abstract void Trigger(Enemy enemy);
}