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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3? clickedCellPos = GameManager.Instance.gridManager.GetCellPosAtPosition(worldPos).GetValueOrDefault();

            if (clickedCellPos.HasValue)
            {
               return MoveTo(clickedCellPos.Value);
            }
            
        }
       return null;
    }
    IEnumerator MoveTo(Vector3 pos)
    {

        transform.position = pos;
        yield return new WaitForEndOfFrame();
    }
    bool WallCheck(Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, 1, 1 << 6);
    }

}
