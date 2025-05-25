using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableObjectData
{
    public GameObject obj;
    public Sprite defaultSprite;
}

public class MapController : MonoBehaviour
{
    [SerializeField] private List<InteractableObjectData> interactableObjects = new List<InteractableObjectData>();

    [Header("Occupied Sprites")]
    [SerializeField] private Sprite bedroomOccupied;
    [SerializeField] private Sprite officeOccupied;
    [SerializeField] private Sprite hallwayOccupied;
    [SerializeField] private Sprite livingRoomOccupied;

    [Header("Room Activation")]
    [SerializeField] private GameObject bedroomObject;
    [SerializeField] private GameObject officeObject;
    [SerializeField] private GameObject hallwayObject;
    [SerializeField] private GameObject livingRoomObject; 

    private void Start()
    {
        RefreshRoomStates();
    }

    public void InteractWith(GameObject obj, string cursorType)
    {
        if (!obj.CompareTag("MapSelectable"))
            return;

        Debug.Log($"MapController: {cursorType} interacted with map object '{obj.name}'.");

        CentralMap.Room roomEnum;
        switch (obj.name)
        {
            case "Bedroom": roomEnum = CentralMap.Room.Bedroom; break;
            case "Hallway": roomEnum = CentralMap.Room.Hallway; break;
            case "Office": roomEnum = CentralMap.Room.Office; break;
            case "LivingRoom": roomEnum = CentralMap.Room.LivingRoom; break;
            default:
                Debug.LogWarning($"Invalid room name '{obj.name}'.");
                return;
        }

        if (CentralMap.Instance.IsRoomOccupied(roomEnum))
        {
            string occupyingCursor = CentralMap.Instance.GetOccupyingCursor(roomEnum);
            if (occupyingCursor != cursorType)
            {
                Debug.Log($"{obj.name} is occupied by {occupyingCursor}. {cursorType} cannot enter.");
                return;
            }
        }

        foreach (var kvp in CentralMap.Instance.GetRoomStates())
        {
            if (kvp.Value == cursorType)
            {
                CentralMap.Instance.ExitRoom(kvp.Key);
                break;
            }
        }

        CentralMap.Instance.EnterRoom(roomEnum, cursorType);

        // Deactivate all rooms
        if (bedroomObject != null) bedroomObject.SetActive(false);
        if (hallwayObject != null) hallwayObject.SetActive(false);
        if (officeObject != null) officeObject.SetActive(false);
        if (livingRoomObject != null) livingRoomObject.SetActive(false); 

        // Activate selected room
        switch (roomEnum)
        {
            case CentralMap.Room.Bedroom:
                if (bedroomObject != null) bedroomObject.SetActive(true);
                break;
            case CentralMap.Room.Hallway:
                if (hallwayObject != null) hallwayObject.SetActive(true);
                break;
            case CentralMap.Room.Office:
                if (officeObject != null) officeObject.SetActive(true);
                break;
            case CentralMap.Room.LivingRoom: 
                if (livingRoomObject != null) livingRoomObject.SetActive(true);
                break;
        }

        SyncAllMaps();
    }

    public void RefreshRoomStates()
    {
        foreach (var data in interactableObjects)
        {
            if (data.obj == null) continue;

            SpriteRenderer sr = data.obj.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            CentralMap.Room roomEnum;
            switch (data.obj.name)
            {
                case "Bedroom": roomEnum = CentralMap.Room.Bedroom; break;
                case "Hallway": roomEnum = CentralMap.Room.Hallway; break;
                case "Office": roomEnum = CentralMap.Room.Office; break;
                case "LivingRoom": roomEnum = CentralMap.Room.LivingRoom; break; 
                default: continue;
            }

            if (CentralMap.Instance.IsRoomOccupied(roomEnum))
            {
                switch (roomEnum)
                {
                    case CentralMap.Room.Bedroom: sr.sprite = bedroomOccupied; break;
                    case CentralMap.Room.Hallway: sr.sprite = hallwayOccupied; break;
                    case CentralMap.Room.Office: sr.sprite = officeOccupied; break;
                    case CentralMap.Room.LivingRoom: sr.sprite = livingRoomOccupied; break; 
                }
            }
            else
            {
                sr.sprite = data.defaultSprite;
            }
        }
    }

    private void SyncAllMaps()
    {
        MapController[] allMaps = FindObjectsOfType<MapController>();
        foreach (var map in allMaps)
        {
            map.RefreshRoomStates();
        }
    }

    private void OnEnable()
    {
        RefreshRoomStates();
    }
}
