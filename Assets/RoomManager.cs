using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<Room> rooms;
    private Room currentRoom = null;
    private int currentRoomIndex;
    private void Start()
    {
        LoadRoom(rooms[0]);
    }
    public void LoadNextRoom()
    {
        currentRoomIndex++;
        if(currentRoomIndex >= rooms.Count)
        {
            currentRoomIndex = 0;
        }
        LoadRoom(rooms[currentRoomIndex]);
    }
    void LoadRoom(Room room)
    {
        if (currentRoom != null)
        {
            GameManager.Instance.ShowAddGlyphUI(GameManager.Instance.player.playerAttack.ability, currentRoom.reward);
            Destroy(currentRoom.gameObject);
        }
        currentRoom = Instantiate(room, Vector3.zero, Quaternion.identity);

        GameManager.Instance.gridManager.SwitchRoom(currentRoom.walkable, currentRoom.collidable, currentRoom.bounds);
       GameManager.Instance.SpawnPlayer(currentRoom.playerSpawnpoint);
        GameManager.Instance.enemyManager.SwitchRoom(currentRoom.enemySpawnpoints);
        GameManager.Instance.glyphManager.SwitchRoom();
        GameManager.Instance.roundManager.SwitchRoom(); 
    }
}
