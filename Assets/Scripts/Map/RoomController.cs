using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactableObjects;
    [SerializeField] private DialogueManager dialogueManager;

    public void InteractWith(GameObject obj, string cursorType)
    {
        if (interactableObjects.Contains(obj))
        {
            if (dialogueManager != null)
            {
                dialogueManager.InteractWith(obj, cursorType);
            }
            else
            {
                Debug.LogWarning($"DialogueManager is not assigned in RoomController on '{gameObject.name}'. Cannot interact with '{obj.name}'.");
            }
        }
        else
        {
            Debug.Log($"Object '{obj.name}' is not managed by the room '{gameObject.name}'.");
        }
    }
}
