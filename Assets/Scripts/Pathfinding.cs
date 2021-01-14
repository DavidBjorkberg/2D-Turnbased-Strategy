using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public float movementSpeed;
    public float getNewPathRate;
    public GridManager gridManager;
    public bool drawPath;
    private List<Vector3> path = new List<Vector3>();
    private int curPathIndex = 0;
    private float getPathTimer;
    private bool isPathing;
    private float pathDistanceThreshold = 0.1f;
    private Transform playerTransform;
    private void Start()
    {
        playerTransform = GameManager.Instance.player.transform;
        GetNewPath();
    }
    void Update()
    {
        if (drawPath)
        {
            DrawPath();
        }

        UpdatePath();

        if (curPathIndex >= 0 && !isPathing)
        {
            StartCoroutine(MoveToNext());
        }
    }
    void UpdatePath()
    {
        getPathTimer -= Time.deltaTime;
        if (GameManager.Instance.gridManager.IsInNeighbourCell(transform.position, playerTransform.position))
        {
            path.Clear();
            path.Add(playerTransform.position);
            curPathIndex = 0;
        }
        else
        {
            if (getPathTimer < 0 || curPathIndex < 0)
            {
                GetNewPath();
                getPathTimer = getNewPathRate;
            }
        }
    }
    void GetNewPath()
    {
        path = gridManager.GetPath(transform.position, GameManager.Instance.player.transform.position);
        curPathIndex = path.Count - 1;
    }

    IEnumerator MoveToNext()
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
    void DrawPath()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Debug.DrawLine(path[path.Count - i - 1], path[path.Count - i - 2]);
        }
    }
}
