using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExplodeGlyph : Glyph
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private int damage;
    protected override void Trigger(Enemy enemy)
    {
        enemy.enemyHealth.TakeDamage(damage);
        Vector3 cellPos = GameManager.Instance.gridManager.GetCellPos(cellIndex);
        GameObject instantiatedExplosion = Instantiate(explosion, cellPos, Quaternion.identity);
        Destroy(instantiatedExplosion, 1);
        GameManager.Instance.glyphManager.RemoveGlyph(cellIndex);
    }
}
