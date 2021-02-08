using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
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
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            damageFlash.Flash();
        }

    }
    public bool IsAlive()
    {
        return curHealth > 0;
    }
}
