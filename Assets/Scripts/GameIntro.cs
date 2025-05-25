using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    // Handles the intro of the game and spawns the rooms for the keyboard and mouse cursors

    [SerializeField] GameObject jounalistPanel;
    [SerializeField] GameObject detectivePanel;
    [SerializeField] GameObject timerScript;
    [SerializeField] float waitDuration;

    [SerializeField] private ObjectRandomizer objectRandomizerForKeyboard;
    [SerializeField] private ObjectRandomizer objectRandomizerForMouse;

    [Header("Rooms")]
    [SerializeField] private List<InteractableObjectData> LeftSide = new List<InteractableObjectData>();
    [SerializeField] private List<InteractableObjectData> RightSide = new List<InteractableObjectData>();

    [Header("Find")]
    [SerializeField] private GameObject leftFindPanel;
    [SerializeField] private GameObject rightFindPanel;

    private void Start()
    {
        jounalistPanel.SetActive(true);
        detectivePanel.SetActive(true);


        StartCoroutine(HandleIntro());
    }

    private IEnumerator HandleIntro()
    {
        yield return new WaitForSeconds(waitDuration);

        jounalistPanel.SetActive(false);
        detectivePanel.SetActive(false);
        timerScript.GetComponent<CountDownTimer>().StartTimer();

        leftFindPanel.SetActive(true);
        rightFindPanel.SetActive(true);

        SpawnRandom();

        objectRandomizerForKeyboard.AssignRandom();
        objectRandomizerForMouse.AssignRandom();
    }

    public void SpawnRandom()
    {
        if (LeftSide.Count == 0 || RightSide.Count == 0)
        {
            Debug.LogWarning("LeftSide or RightSide room lists are empty.");
            return;
        }

        foreach (var data in LeftSide) data.obj.SetActive(false);
        foreach (var data in RightSide) data.obj.SetActive(false);

        CentralMap.Instance = FindObjectOfType<CentralMap>();
        CentralMap.Instance?.GetRoomStates().Clear();

        int leftIndex = Random.Range(0, LeftSide.Count);
        InteractableObjectData leftRoomData = LeftSide[leftIndex];
        leftRoomData.obj.SetActive(true);

        CentralMap.Room leftRoomEnum = GetRoomEnum(leftRoomData.obj.name);
        CentralMap.Instance.EnterRoom(leftRoomEnum, "Keyboard");

        InteractableObjectData rightRoomData = null;
        CentralMap.Room rightRoomEnum = CentralMap.Room.Bedroom;
        int attempts = 0;

        do
        {
            int rightIndex = Random.Range(0, RightSide.Count);
            rightRoomData = RightSide[rightIndex];
            rightRoomEnum = GetRoomEnum(rightRoomData.obj.name);
            attempts++;
        } while ((rightRoomEnum == leftRoomEnum || CentralMap.Instance.IsRoomOccupied(rightRoomEnum)) && attempts < 10);

        rightRoomData.obj.SetActive(true);
        CentralMap.Instance.EnterRoom(rightRoomEnum, "Mouse");

        Debug.Log($"SpawnRandom: Keyboard in {leftRoomData.obj.name}, Mouse in {rightRoomData.obj.name}");

        CentralMap.Room GetRoomEnum(string roomName)
        {
            return roomName switch
            {
                "Bedroom" => CentralMap.Room.Bedroom,
                "Hallway" => CentralMap.Room.Hallway,
                "Office" => CentralMap.Room.Office,
                "LivingRoom" => CentralMap.Room.LivingRoom, 
                _ => throw new System.Exception($"Unknown room name: {roomName}")
            };
        }
    }
}
