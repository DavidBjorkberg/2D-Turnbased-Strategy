using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Tilemap walkable;
    public Tilemap collidable;
    public Tilemap enemySpawnpoints;
    public Tilemap playerSpawnpoint;
    public Tilemap bounds;
    public Glyph reward;

}
