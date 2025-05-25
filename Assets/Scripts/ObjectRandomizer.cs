using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI findObject;
    [SerializeField] private GameObject cursor;

    [Header("Bedroom Objects")]
    [SerializeField] private List<GameObject> bedroomObjects = new List<GameObject>();

    [Header("Office Objects")]
    [SerializeField] private List<GameObject> officeObjects = new List<GameObject>();

    [Header("Hallway Objects")]
    [SerializeField] private List<GameObject> hallwayObjects = new List<GameObject>();

    [Header("Living Room Objects")] 
    [SerializeField] private List<GameObject> livingRoomObjects = new List<GameObject>(); 

    public void AssignRandom()
    {
        List<GameObject> allObjects = new List<GameObject>();

        allObjects.AddRange(bedroomObjects);
        allObjects.AddRange(officeObjects);
        allObjects.AddRange(hallwayObjects);
        allObjects.AddRange(livingRoomObjects); 

        // Filter only active objects
        List<GameObject> validObjects = allObjects.FindAll(obj => obj != null && obj.activeSelf);

        if (validObjects.Count == 0)
        {
            findObject.text = "All objects taken.";
            Debug.Log("AssignRandom: No valid objects remaining.");
            return;
        }

        int randomIndex = Random.Range(0, validObjects.Count);
        GameObject selectedObject = validObjects[randomIndex];

        findObject.text = $"Find: {selectedObject.name}";
        Debug.Log($"AssignRandom: Cursor '{cursor.tag}' assigned to find {selectedObject.name}");
    }

    public void RemoveObject(GameObject obj)
    {
        bedroomObjects.Remove(obj);
        officeObjects.Remove(obj);
        hallwayObjects.Remove(obj);
        livingRoomObjects.Remove(obj); 
    }
}
