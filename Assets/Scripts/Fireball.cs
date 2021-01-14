using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public float speed;
    private Vector3 direction;
    private bool isInitialized;
    public void Initialize(Vector3 direction)
    {
        this.direction = direction;
        isInitialized = true;
        transform.right = direction;
    }

    private void Update()
    {
        if (isInitialized)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth player))
        {
            player.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
