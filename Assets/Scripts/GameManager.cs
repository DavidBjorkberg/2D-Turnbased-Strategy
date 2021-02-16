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
    [SerializeField] private AddGlyphUI AddGlyphUI;
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
        BoundsInt bounds = gridManager.gridBounds;
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMax - 1, j = 0; j < (bounds.size.y); y--, j++)
            {
                if (playerSpawnpoint.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector2Int spawnPointIndex = new Vector2Int(i, j);
                    Vector3 spawnPos = gridManager.GetCellPos(spawnPointIndex);
                    if (player == null)
                    {
                        player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                    }
                    else
                    {
                        player.transform.position = spawnPos;
                    }
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
    public void ShowAddGlyphUI(PlayerAbility ability, Glyph glyphToAdd)
    {
        AddGlyphUI.Show(ability, glyphToAdd);
    }
    public bool IsCellFree(Vector2Int cellIndex)
    {
        return gridManager.IsCellFree(cellIndex) && enemyManager.IsCellFreeFromEnemies(cellIndex);
    }


}