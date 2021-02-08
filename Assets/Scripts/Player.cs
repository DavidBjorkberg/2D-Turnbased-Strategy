using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public AddGlyphUI AddGlyphUI;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddGlyphUI.Show(playerAttack.ability, GameManager.Instance.testGlyph);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddGlyphUI.Hide();
        }

    }
}
