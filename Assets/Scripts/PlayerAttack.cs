using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackButton;
    public GameObject attackRect;
    private PlayerMovement movement;
    private List<List<AttackCell>> attackGrid = new List<List<AttackCell>>();
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            attackGrid.Add(new List<AttackCell>());
            for (int j = 0; j < 3; j++)
            {
                AttackCell cell = ScriptableObject.CreateInstance("AttackCell") as AttackCell;
                cell.isActive = true;
                if (i == 2 && j == 2 || i == 1 && j == 1 || i == 1 && j == 2)
                {
                    cell.attackRect = attackRect;
                }
                else
                {
                    cell.attackRect = null;
                }
                attackGrid[i].Add(cell);
            }
        }
        movement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            Attack();
        }
    }

    void Attack()
    {
        Vector3 topLeftPos = transform.position + new Vector3(-attackGrid.Count + 1, attackGrid[0].Count - 1);
        for (int i = 0; i < attackGrid.Count; i++)
        {
            for (int j = 0; j < attackGrid[i].Count; j++)
            {
                if (attackGrid[i][j].attackRect != null)
                {
                    Vector3 spawnPos = topLeftPos + new Vector3(j * 2, -i * 2);
                    Instantiate(attackGrid[i][j].attackRect, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
