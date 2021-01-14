using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public float iFrameLength;
    private int curHealth;
    private Player player;
    private bool isInvincible;
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
        if (!isInvincible)
        {
            curHealth -= amount;
            if (curHealth <= 0)
            {
                Died();
            }
            damageFlash.Flash();
            StartCoroutine(IFrame());
        }
    }
    void Died()
    {
        player.spriteRenderer.enabled = false;
        player.hitbox.enabled = false;
    }
    IEnumerator IFrame()
    {
        float iFrameTimer = iFrameLength;
        isInvincible = true;

        while (iFrameTimer > 0)
        {
            iFrameTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isInvincible = false;

    }
}
