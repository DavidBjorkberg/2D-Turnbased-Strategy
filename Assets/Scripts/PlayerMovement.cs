using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private PlayerAttack playerAttack;
    private Vector2Int currentCellIndex;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    private void Start()
    {
        Vector2Int? cellIndexNullable = GameManager.Instance.gridManager.GetCellAtPosition(transform.position);
        currentCellIndex = cellIndexNullable.Value;
        if(!cellIndexNullable.HasValue)
        {
            Debug.LogError("Player must start inside grid");
        }
    }
    void Update()
    {
        Shader.SetGlobalVector("playerPosition", new Vector4(transform.position.x, transform.position.y, transform.position.z, 1));
    }
    public IEnumerator RequestAction()
    {
        if (Input.GetMouseButton(0) && !playerAttack.attackGrid.IsGridActive())
        {
            Vector2 mousePos = GameManager.Instance.GetMousePosInWorld();
            Vector2? clickedCellPos = GameManager.Instance.gridManager.GetCellPosAtPosition(mousePos);
            if (clickedCellPos.HasValue)
            {
                if (GameManager.Instance.gridManager.IsInRange(transform.position, clickedCellPos.Value, 1))
                {
                    return MoveTo(clickedCellPos.Value);

                }
            }

        }
        return null;
    }
    public Vector2Int GetCurrentCellIndex()
    {
        return currentCellIndex;
    }
    IEnumerator MoveTo(Vector3 pos)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = pos;
        float lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, lerpValue);
            yield return new WaitForEndOfFrame();
        }
        Vector2Int? cellIndexNullable = GameManager.Instance.gridManager.GetCellAtPosition(transform.position);
        currentCellIndex = cellIndexNullable.Value;
    }
}
