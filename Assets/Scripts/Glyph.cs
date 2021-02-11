using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Glyph/Glyph")]

public class Glyph : ScriptableObject
{
    public Sprite sprite;
    [SerializeField] private GameObject explosion;
    internal Vector2Int cellIndex;
    public void Process(bool endOfTurn)
    {
        CheckCondition();
    }
    void CheckCondition()
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
    void Trigger(Enemy enemy)
    {
        enemy.enemyHealth.TakeDamage(3);

        Vector3 cellPos = GameManager.Instance.gridManager.GetCellPos(cellIndex);
        GameObject instantiatedExplosion = Instantiate(explosion, cellPos, Quaternion.identity);
        Destroy(instantiatedExplosion, 1);
        GameManager.Instance.glyphManager.RemoveGlyph(cellIndex);
    }
}