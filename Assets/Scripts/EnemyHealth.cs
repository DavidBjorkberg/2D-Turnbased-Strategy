using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int curHealth;
    private TakeDamageFlash damageFlash;
    private void Awake()
    {
        curHealth = maxHealth;
        damageFlash = GetComponent<TakeDamageFlash>();
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            damageFlash.Flash();
        }

    }
}
