using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyCombat : MonoBehaviour
{
    public Fireball fireball;
    public float range;
    public float attackCooldown;
    private float attackTimer;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (GetDistanceToPlayer() <= range && attackTimer <= 0)
        {
            Shoot();
            attackTimer = attackCooldown;
        }
    }
    void Shoot()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        Fireball instantiatedFireball = Instantiate(fireball, transform.position, Quaternion.identity);
        instantiatedFireball.Initialize(direction);

    }

    float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, playerTransform.position);

    }
}
