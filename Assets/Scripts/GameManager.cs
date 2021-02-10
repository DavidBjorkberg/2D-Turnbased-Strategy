using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    internal Player player;
    public GridManager gridManager;
    public RoundManager roundManager;
    public GlyphManager glyphManager;
    public EnemyManager enemyManager;
    public RoomManager roomManager;
    public Glyph testGlyph;
    [SerializeField] private Player playerPrefab;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void SpawnPlayer(Tilemap playerSpawnpoint)
    {
        if(player != null)
        {
            Destroy(player.gameObject);
        }
        BoundsInt bounds = gridManager.gridBounds;
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMax - 1, j = 0; j < (bounds.size.y); y--, j++)
            {
                if (playerSpawnpoint.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector2Int spawnPointIndex = new Vector2Int(i, j);
                    Vector3 spawnPos = gridManager.GetCellPos(spawnPointIndex);
                    player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                    break;
                }
            }
        }
    }
    private void Update()
    {
        Shader.SetGlobalVector("mousePos", new Vector4(GetMousePosInWorld().x, GetMousePosInWorld().y, 0, 1));
    }
    public Vector2 GetMousePosInWorld()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPos;
    }
    public Vector2Int? GetFreeCell()
    {
        Vector2Int gridSize = gridManager.GetGridSize();
        Vector2Int curCellIndex;
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                curCellIndex = new Vector2Int(i, j);
                if (gridManager.IsCellFree(curCellIndex)
                    && player.playerMovement.GetCurrentCellIndex() != curCellIndex
                    && enemyManager.IsCellFreeFromEnemies(curCellIndex))
                {
                    return curCellIndex;
                }
            }
        }
        return null;
    }
    public List<List<Glyph>> DeepCopyGrid(List<List<Glyph>> grid)
    {
        List<List<Glyph>> newGrid = new List<List<Glyph>>();
        for (int i = 0; i < grid.Count; i++)
        {
            newGrid.Add(new List<Glyph>());
            for (int j = 0; j < grid[i].Count; j++)
            {
                if (grid[i][j] != null)
                {
                    newGrid[i].Add(Instantiate(grid[i][j]));
                }
                else
                {
                    newGrid[i].Add(null);
                }
            }
        }
        return newGrid;
    }

}