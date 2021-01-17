using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public enum FacingDir { Right, Left };
    public FacingDir facingDir;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        facingDir = FacingDir.Left;
    }
    void Update()
    {
        Shader.SetGlobalVector("playerPosition", new Vector4(transform.position.x, transform.position.y, transform.position.z, 1));
        Shader.SetGlobalVector("mousePos", new Vector4(GameManager.Instance.GetMousePosInWorld().x, GameManager.Instance.GetMousePosInWorld().y, 0, 1));
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontalInput, verticalInput);
        dir.Normalize();

        if (horizontalInput > 0 && facingDir != FacingDir.Right)
        {
            spriteRenderer.flipX = false;
            facingDir = FacingDir.Right;
        }
        else if (horizontalInput < 0 && facingDir != FacingDir.Left)
        {
            spriteRenderer.flipX = true;
            facingDir = FacingDir.Left;
        }

        if (!WallCheck(dir))
        {
            transform.position += dir * movementSpeed * Time.deltaTime;
        }
    }
    public IEnumerator RequestAction()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = GameManager.Instance.GetMousePosInWorld();
            Vector2? clickedCellPos = GameManager.Instance.gridManager.GetCellPosAtPosition(mousePos);
            if (clickedCellPos.HasValue)
            {
                if (GameManager.Instance.gridManager.IsInNeighbourCell(transform.position, clickedCellPos.Value))
                {
                    return MoveTo(clickedCellPos.Value);

                }
            }

        }
        return null;
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
    }
    bool WallCheck(Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, 1, 1 << 6);
    }

}
