using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
   [SerializeField] private float movementSpeed;
   [SerializeField] private bool drawPath;

    protected GridManager gridManager;
    protected Transform playerTransform;
    protected List<Vector3> path = new List<Vector3>();

    private void Start()
    {
        gridManager = GameManager.Instance.gridManager;
        playerTransform = GameManager.Instance.player.transform;
        GetNewPath();
    }
    void Update()
    {
        if (drawPath)
        {
            DrawPath();
        }
    }

    public virtual IEnumerator RequestAction()
    {
        GetNewPath();
        return MoveToNext();
    }

    protected void GetNewPath()
    {
        path = gridManager.GetPath(transform.position, GameManager.Instance.player.transform.position);

        if (path.Count > 0)
        {
            path.RemoveAt(path.Count - 1);
            GetComponent<Enemy>().SetClaimedCellIndex(GameManager.Instance.gridManager.GetCellAtPosition(path[path.Count - 1]).Value);
        }
    }
    protected IEnumerator MoveToNext()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = path[path.Count - 1];
        float lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, lerpValue);
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Enemy>().SetCurrentCellIndex(GameManager.Instance.gridManager.GetCellAtPosition(endPos).Value);
        path.RemoveAt(path.Count - 1);
    }
    protected void DrawPath()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Debug.DrawLine(path[path.Count - i - 1], path[path.Count - i - 2]);
        }
    }
}
