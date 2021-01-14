using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRect : MonoBehaviour
{
    public float lifeTime;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.TakeDamage(5);
        }
    }
}
