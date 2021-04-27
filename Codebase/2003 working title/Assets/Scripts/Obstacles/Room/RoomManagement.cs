using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagement : MonoBehaviour
{
    List<RoomHandle> Rooms = new List<RoomHandle>();
    [SerializeField] RoomHandle StartRoom = null;
    [SerializeField] RoomHandle currRoom = null;
    [SerializeField] CheckPoints currCheckpoint = null;
    [SerializeField] Exit[] Entrances = null;
    Vector2 levelStart;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<LocationTracker>())
        {
            Exit Temp = Entrances[FindObjectOfType<LocationTracker>().door];
            FindObjectOfType<PlayerController2D>().transform.position = Temp.ExitLoc.position;
            if (Temp.MyRoom)
            {
                StartRoom = Temp.MyRoom;
            }
        }
        else
        {
            GameObject Temp = new GameObject();
            Temp.AddComponent<LocationTracker>();
            DontDestroyOnLoad(Temp);
        }
        levelStart = FindObjectOfType<PlayerController2D>().transform.position;
     
        RoomHandle[] temp = FindObjectsOfType<RoomHandle>();
        if (temp.Length > 0)
        {
            foreach (RoomHandle room in temp)
            {
                Rooms.Add(room);
            }
        }
        currRoom = StartRoom;
        currRoom.Load();
        FindObjectOfType<PlayerController2D>().UpdateCollisions();
    }
    public void UpdateRoom(RoomHandle newRoom)
    {
        currRoom.Unload();
        currRoom = newRoom;
        currRoom.Load();
        FindObjectOfType<PlayerController2D>().UpdateCollisions();
    }
    public void UpdateRoom(CheckPoints RespawnPoint)
    {
        Time.timeScale = 0;
        currRoom.Unload();
        currRoom = RespawnPoint.MyRoom;
        currRoom.Load();
        FindObjectOfType<PlayerController2D>().transform.position = RespawnPoint.gameObject.transform.position;
        FindObjectOfType<PlayerController2D>().UpdateCollisions();
        Time.timeScale = 1;
    }
    //If there is no spawn point sen the player to the begining of the room
    void UpdateRoom()
    {
        Time.timeScale = 0;
        currRoom.Unload();
        currRoom = StartRoom;
        currRoom.Load();
        FindObjectOfType<PlayerController2D>().transform.position = levelStart;
        FindObjectOfType<PlayerController2D>().UpdateCollisions();
        Time.timeScale = 1;
    }
    public void SetRespawn(Vector2 pos)
    {
        levelStart = pos;
    }
    public void SetRespawn(CheckPoints newCheck)
    {
        if (currCheckpoint != null)
        {
            currCheckpoint.SetEnabled();
        }
        currCheckpoint = newCheck;
        currCheckpoint.SetEnabled();
    }
    public void Respawn()
    {
        if(currCheckpoint == null)
        {
            UpdateRoom();
        }
        else
        {
            UpdateRoom(currCheckpoint);
        }
    }
    public RoomHandle GetCurrentRoom()
    {
        return currRoom;
    }
}
