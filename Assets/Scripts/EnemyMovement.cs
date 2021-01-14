using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed;
    public float getNewPathRate;
    public bool drawPath;

    protected GridManager gridManager;
    protected Transform playerTransform;
    protected List<Vector3> path = new List<Vector3>();
    protected float getPathTimer;
    protected int curPathIndex = 0;
    private bool isPathing;
    private float pathDistanceThreshold = 0.1f;

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
        MovementUpdate();
    }
    protected virtual void MovementUpdate()
    {
        UpdatePath();

        if (ShouldPath())
        {
            StartCoroutine(MoveToNext());
        }
    }
    protected virtual bool ShouldPath()
    {
        return curPathIndex >= 0 && !isPathing;
    }
    protected virtual void UpdatePath()
    {
        getPathTimer -= Time.deltaTime;

        if (getPathTimer < 0 || curPathIndex < 0)
        {
            GetNewPath();
            getPathTimer = getNewPathRate;
        }
    }
    protected void GetNewPath()
    {
        path = gridManager.GetPath(transform.position, GameManager.Instance.player.transform.position);
        if (path.Count > 0)
        {
            curPathIndex = path.Count - 1;
        }
    }
    protected IEnumerator MoveToNext()
    {
        isPathing = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = path[curPathIndex];
        Vector3 direction = (endPos - startPos).normalized;

        while (Vector3.Distance(transform.position, endPos) >= pathDistanceThreshold)
        {
            transform.position += direction * movementSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        curPathIndex--;
        isPathing = false;
    }
    protected void DrawPath()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Debug.DrawLine(path[path.Count - i - 1], path[path.Count - i - 2]);
        }
    }
}
