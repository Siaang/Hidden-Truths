using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Manages the whole dialogue system of the game including animations and text

    private ObjectDialogue currentObjectDialogue;

    [Header("Dialogue Boxes")]
    [SerializeField] private GameObject dbMouse;
    [SerializeField] private GameObject dbKeyboard;

    [Header("Text Fields")]
    [SerializeField] private TMP_Text dialogueTextMouse;
    [SerializeField] private TMP_Text dialogueTextKeyboard;

    [Header("Animator")]
    [SerializeField] private Animator animatorKeyboard;
    [SerializeField] private Animator animatorMouse;

    [Header("Object Randomizers")]
    [SerializeField] private ObjectRandomizer mouseRandomizer;
    [SerializeField] private ObjectRandomizer keyboardRandomizer;

    /*
    [Header("Decision Panels")]
    [SerializeField] private ObjectChoicePanel objectChoicePanelMouse;
    [SerializeField] private ObjectChoicePanel objectChoicePanelKeyboard;
    */

    private Queue<string> sentencesMouse;
    private Queue<string> sentencesKeyboard;

    public bool IsMouseDialogueOpen { get; private set; }
    public bool IsKeyboardDialogueOpen { get; private set; }

    private string lastInteractedCursor = "";

    private void Start()
    {
        sentencesMouse = new Queue<string>();
        sentencesKeyboard = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowNextSentenceKeyboard();
        }
    }

    public void InteractWith(GameObject obj, string cursorType)
    {
        ObjectDialogue dialogueData = obj.GetComponent<ObjectDialogue>();
        if (dialogueData == null)
        {
            Debug.LogWarning($"No ObjectDialogue script found on {obj.name}");
            return;
        }

        currentObjectDialogue = dialogueData;
        lastInteractedCursor = cursorType;

        if (cursorType == "Mouse")
        {
            dialogueData.Init(mouseRandomizer, "Mouse");

            sentencesMouse.Clear();
            foreach (string sentence in dialogueData.GetSentences())
            {
                sentencesMouse.Enqueue(sentence);
            }

            ShowNextSentenceMouse();
            animatorMouse.SetBool("IsOpen_Mouse", true);
        }
        else if (cursorType == "Keyboard")
        {
            dialogueData.Init(keyboardRandomizer, "Keyboard");

            sentencesKeyboard.Clear();
            foreach (string sentence in dialogueData.GetSentences())
            {
                sentencesKeyboard.Enqueue(sentence);
            }

            ShowNextSentenceKeyboard();
            animatorKeyboard.SetBool("IsOpen", true);
        }
    }

    public void ShowNextSentenceMouse()
    {
        if (sentencesMouse.Count == 0)
        {
            EndDialogueMouse();
            return;
        }

        dbMouse.SetActive(true);
        IsMouseDialogueOpen = true;
        dialogueTextMouse.text = sentencesMouse.Dequeue();
    }

    public void ShowNextSentenceKeyboard()
    {
        if (sentencesKeyboard.Count == 0)
        {
            EndDialogueKeyboard();
            return;
        }

        dbKeyboard.SetActive(true);
        IsKeyboardDialogueOpen = true;
        dialogueTextKeyboard.text = sentencesKeyboard.Dequeue();
    }

    private void EndDialogueMouse()
    {
        Debug.Log("End of mouse dialogue.");
        animatorMouse.SetBool("IsOpen_Mouse", false);
        IsMouseDialogueOpen = false;
        dbMouse.SetActive(false);

        currentObjectDialogue?.EndSentence();

        /*
        if (currentObjectDialogue != null)
        {
            StartCoroutine(ShowDecisionPanelNextFrame(currentObjectDialogue, "Mouse"));
        }
        */
    }

    private void EndDialogueKeyboard()
    {
        Debug.Log("End of keyboard dialogue.");
        animatorKeyboard.SetBool("IsOpen", false);
        IsKeyboardDialogueOpen = false;
        dbKeyboard.SetActive(false);

        currentObjectDialogue?.EndSentence();

        /*
        if (currentObjectDialogue != null)
        {
            StartCoroutine(ShowDecisionPanelNextFrame(currentObjectDialogue, "Keyboard"));
        }
        */
    }

    /*
    private IEnumerator ShowDecisionPanelNextFrame(ObjectDialogue dialogue, string cursorType)
    {
        yield return null; 

        if (cursorType == "Mouse")
        {
            objectChoicePanelMouse.Initialize(dialogue);
        }
        else if (cursorType == "Keyboard")
        {
            objectChoicePanelKeyboard.Initialize(dialogue);
        }
    }
    */
}
