using UnityEngine;
using System.Collections.Generic;

public class CentralMap : MonoBehaviour
{
    public static CentralMap Instance;

    public enum Room
    {
        Bedroom,
        Hallway,
        Office,
        LivingRoom 
    }

    private Dictionary<Room, string> occupiedRooms = new Dictionary<Room, string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsRoomOccupied(Room room)
    {
        return occupiedRooms.ContainsKey(room);
    }

    public string GetOccupyingCursor(Room room)
    {
        return occupiedRooms.ContainsKey(room) ? occupiedRooms[room] : null;
    }

    public void EnterRoom(Room room, string cursorType)
    {
        occupiedRooms[room] = cursorType;
    }

    public void ExitRoom(Room room)
    {
        if (occupiedRooms.ContainsKey(room))
            occupiedRooms.Remove(room);
    }

    public Dictionary<Room, string> GetRoomStates()
    {
        return new Dictionary<Room, string>(occupiedRooms);
    }
}
