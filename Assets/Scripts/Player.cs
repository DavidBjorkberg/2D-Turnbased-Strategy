using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    internal PlayerAttack playerAttack;
    internal PlayerMovement playerMovement;
    internal PlayerHealth playerHealth;
    internal BoxCollider2D hitbox;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        hitbox = GetComponent<BoxCollider2D>();
    }
}
