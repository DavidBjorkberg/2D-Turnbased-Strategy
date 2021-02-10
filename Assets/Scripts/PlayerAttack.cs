using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AttackGrid attackGrid;
    [SerializeField] private KeyCode attackButton;
    internal PlayerAbility ability;
    private void Start()
    {
        ability = ScriptableObject.CreateInstance("PlayerAbility") as PlayerAbility;
    }
    public void PerformNonTurnEndingAction()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (attackGrid.IsGridActive())
            {
                attackGrid.DeactivateGrid();
            }
            else
            {
                attackGrid.ActivateGrid(GameManager.Instance.DeepCopyGrid(ability.grid));
            }
        }
    }
    public IEnumerator RequestAction()
    {
        if (attackGrid.IsGridActive() && Input.GetMouseButtonDown(0))
        {
            return Attack();
        }
        return null;
    }
    IEnumerator Attack()
    {
        List<List<Glyph>> rotatedAbilityGrid = new List<List<Glyph>>();
        List<List<Vector2Int>> nearbyIndices = new List<List<Vector2Int>>();

        rotatedAbilityGrid = attackGrid.GetRotatedGrid();
        nearbyIndices = GameManager.Instance.gridManager.GetNearbyCellIndicesAsGrid(transform.position, 3);

        for (int i = 0; i < rotatedAbilityGrid.Count; i++)
        {
            for (int j = 0; j < rotatedAbilityGrid[i].Count; j++)
            {
                if (rotatedAbilityGrid[i][j] != null)
                {
                    GameManager.Instance.glyphManager.PlaceGlyph(rotatedAbilityGrid[i][j], nearbyIndices[i][j]);
                }
            }
        }

        attackGrid.DeactivateGrid();
        yield return null;
    }
}
