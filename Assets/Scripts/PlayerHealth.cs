using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int curHealth;
    private Player player;
    private TakeDamageFlash damageFlash;

    private void Awake()
    {
        player = GetComponent<Player>();
        curHealth = maxHealth;
    }
    private void Start()
    {
        damageFlash = GetComponent<TakeDamageFlash>();
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        if (curHealth <= 0)
        {
            Died();
        }
        damageFlash.Flash();
    }
    void Died()
    {
        player.spriteRenderer.enabled = false;
        player.hitbox.enabled = false;
    }
}
