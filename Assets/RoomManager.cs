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
        LoadRoom(rooms[currentRoomIndex]);
    }
    void LoadRoom(Room room)
    {
        if(currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }
        currentRoom = Instantiate(room,Vector3.zero,Quaternion.identity);

        GameManager.Instance.gridManager.SwitchRoom(currentRoom.walkable, currentRoom.collidable, currentRoom.bounds);
        GameManager.Instance.enemyManager.SwitchRoom(currentRoom.enemySpawnpoints);
        GameManager.Instance.SpawnPlayer(currentRoom.playerSpawnpoint);

        GameManager.Instance.roundManager.StartPlayerTurn();
    }
}
