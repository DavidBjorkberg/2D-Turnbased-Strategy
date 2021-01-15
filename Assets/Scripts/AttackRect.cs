using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRect : MonoBehaviour
{
    public float lifeTime;
    private List<GameObject> hitEnemies = new List<GameObject>();
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hitEnemies.Contains(collision.gameObject) && collision.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.TakeDamage(5);
            hitEnemies.Add(collision.gameObject);
        }
    }
}
