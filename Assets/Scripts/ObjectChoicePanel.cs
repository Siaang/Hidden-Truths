using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectChoicePanel : MonoBehaviour
{
    public enum CursorType { Mouse, Keyboard }

    [SerializeField] private CursorType cursorType;
    [SerializeField] private GameObject panelObject;
    [SerializeField] private UnifiedCursorMovement cursor;

    [Header("Interactive Button Objects")]
    [SerializeField] private GameObject takeButtonObject;
    [SerializeField] private GameObject leaveButtonObject;

    private ObjectDialogue targetDialogue;

    private void Start()
    {
        if (panelObject == null)
            panelObject = gameObject;

        panelObject.SetActive(false);
    }

    public void Initialize(ObjectDialogue dialogue)
    {
        targetDialogue = dialogue;
        panelObject?.SetActive(true);
        cursor?.BlockInteractions(true);

        // Ensure the EventSystem is cleared before setting the new selection
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            // Auto-select the Take Button for keyboard/controller input
            if (takeButtonObject != null)
            {
                EventSystem.current.SetSelectedGameObject(takeButtonObject);
            }
        }
    }


    public void ClosePanel()
    {
        panelObject?.SetActive(false);
        cursor?.BlockInteractions(false);
    }

    public void TriggerTake()
    {
        Debug.Log("Take action triggered.");
        ClosePanel();
    }

    public void TriggerLeave()
    {
        Debug.Log("Leave action triggered.");
        ClosePanel();
    }

    public void HandleButtonPress(GameObject button)
    {
        if (button.CompareTag("TakeButton"))
        {
            // Handle take action
            Debug.Log("Take button pressed");
        }
        else if (button.CompareTag("LeaveButton"))
        {
            // Handle leave action
            Debug.Log("Leave button pressed");
        }
    }

}
