using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class UnifiedCursorMovement : MonoBehaviour
{
    public enum CursorType { Mouse, Keyboard }
    [SerializeField] private CursorType cursorType;
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;

    private bool blockSelectableInteractions = false;
    private Rigidbody2D rb;

    private Color highlightColor = Color.gray;
    private Color originalColor;
    private SpriteRenderer currentSpriteRenderer;
    private GameObject currentObject;

    [Header("Map UI")]
    [SerializeField] private GameObject LeftmapUI;
    [SerializeField] private GameObject RightmapUI;
    [SerializeField] private MapController mapController;

    [Header("Panels")]
    [SerializeField] private GameObject journalistPanel;
    [SerializeField] private GameObject detectivePanel;
    private bool lastPanelsActiveState;

    [SerializeField] private GameObject pausePanel;

    private bool isMapOpen = false;
    private bool isPaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (journalistPanel != null || detectivePanel != null)
        {
            lastPanelsActiveState = AreAnyPanelsActive();
            BlockInteractions(lastPanelsActiveState);
        }
    }

    void Update()
    {
        if (journalistPanel != null || detectivePanel != null)
        {
            bool currentPanelsState = AreAnyPanelsActive();
            if (lastPanelsActiveState != currentPanelsState)
            {
                BlockInteractions(currentPanelsState);
                lastPanelsActiveState = currentPanelsState;
            }
        }

        if (dialogueManager != null)
        {
            if ((cursorType == CursorType.Keyboard && dialogueManager.IsKeyboardDialogueOpen) ||
                (cursorType == CursorType.Mouse && dialogueManager.IsMouseDialogueOpen))
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;

            if (pausePanel != null)
                pausePanel.SetActive(isPaused);
        }

        if (isPaused)
            return;

        if (blockSelectableInteractions)
        {
            if (currentObject == null ||
                (currentObject.CompareTag("Selectable") || currentObject.CompareTag("MapSelectable")))
            {
                return;
            }
        }

        HandleMapToggle();

        if (blockSelectableInteractions)
        {
            return;
        }

        HandleInteractionInput();
    }

    private bool AreAnyPanelsActive()
    {
        return (journalistPanel != null && journalistPanel.activeSelf) ||
               (detectivePanel != null && detectivePanel.activeSelf);
    }

    public void BlockInteractions(bool block)
    {
        blockSelectableInteractions = block;

        if (block)
        {
            if (currentSpriteRenderer != null && currentObject != null)
            {
                if (currentObject.CompareTag("Selectable") || currentObject.CompareTag("MapSelectable"))
                {
                    currentSpriteRenderer.color = originalColor;
                    currentObject = null;
                    currentSpriteRenderer = null;
                }
            }

            if (isMapOpen)
            {
                isMapOpen = false;
                LeftmapUI.SetActive(false);
                RightmapUI.SetActive(false);
            }
        }
    }

    void HandleMapToggle()
    {
        if (blockSelectableInteractions) return;

        if (cursorType == CursorType.Keyboard && Input.GetKeyDown(KeyCode.Q))
        {
            isMapOpen = !isMapOpen;
            LeftmapUI.SetActive(isMapOpen);
        }

        if (cursorType == CursorType.Mouse && Input.GetMouseButtonDown(1))
        {
            isMapOpen = !isMapOpen;
            RightmapUI.SetActive(isMapOpen);
        }
    }

    void HandleInteractionInput()
    {
        // Handle E key on UI Buttons selected via EventSystem
        if (currentObject == null)
        {
            if (cursorType == CursorType.Keyboard && Input.GetKeyDown(KeyCode.E))
            {
                GameObject selectedGO = EventSystem.current.currentSelectedGameObject;
                if (selectedGO != null)
                {
                    var button = selectedGO.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke();
                        return;
                    }
                }
            }
        }

        bool keyboardPressed = cursorType == CursorType.Keyboard && Input.GetKeyDown(KeyCode.E);
        bool mousePressed = cursorType == CursorType.Mouse && Input.GetMouseButtonDown(0);

        if (!keyboardPressed && !mousePressed)
        {
            return;
        }

        if (currentObject == null)
        {
            return;
        }

        if (currentObject.CompareTag("TakeButton") || currentObject.CompareTag("LeaveButton"))
        {
            var button = currentObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
        else if (isMapOpen && currentObject.CompareTag("MapSelectable"))
        {
            mapController?.InteractWith(currentObject, cursorType.ToString());
        }
        else if (!isMapOpen && currentObject.CompareTag("Selectable"))
        {
            RoomController roomController = currentObject.GetComponentInParent<RoomController>();
            if (roomController != null)
            {
                roomController.InteractWith(currentObject, cursorType.ToString());
            }
        }
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;

        if (cursorType == CursorType.Mouse)
            MoveWithMouse();
        else if (cursorType == CursorType.Keyboard)
            MoveWithKeyboard();
    }

    void MoveWithMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 targetPosition = new Vector2(mousePos.x, mousePos.y);
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }

    void MoveWithKeyboard()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dialogueManager != null &&
            ((cursorType == CursorType.Keyboard && dialogueManager.IsKeyboardDialogueOpen) ||
             (cursorType == CursorType.Mouse && dialogueManager.IsMouseDialogueOpen)))
            return;

        if (blockSelectableInteractions &&
            (collision.CompareTag("Selectable") || collision.CompareTag("MapSelectable") || collision.CompareTag("TakeButton") || collision.CompareTag("LeaveButton")))
            return;

        bool isMapElement = collision.CompareTag("MapSelectable");
        bool isWorldElement = collision.CompareTag("Selectable");
        bool isButton = collision.CompareTag("TakeButton") || collision.CompareTag("LeaveButton");

        if (isMapOpen && !(isMapElement || isButton)) return;
        if (!isMapOpen && !(isWorldElement || isButton)) return;

        if (currentObject == collision.gameObject) return;

        if (currentObject != null && currentSpriteRenderer != null)
        {
            currentSpriteRenderer.color = originalColor;
        }

        currentObject = collision.gameObject;
        currentSpriteRenderer = currentObject.GetComponentInChildren<SpriteRenderer>();

        if (currentSpriteRenderer != null)
        {
            originalColor = currentSpriteRenderer.color;

            if (!blockSelectableInteractions || isButton)
                currentSpriteRenderer.color = highlightColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isMapElement = collision.CompareTag("MapSelectable");
        bool isWorldElement = collision.CompareTag("Selectable");
        bool isButton = collision.CompareTag("TakeButton") || collision.CompareTag("LeaveButton");

        if ((isMapOpen && (isMapElement || isButton)) || (!isMapOpen && (isWorldElement || isButton)))
        {
            if (collision.gameObject == currentObject && currentSpriteRenderer != null)
            {
                currentSpriteRenderer.color = originalColor;
                currentObject = null;
                currentSpriteRenderer = null;
            }
        }
    }
}
