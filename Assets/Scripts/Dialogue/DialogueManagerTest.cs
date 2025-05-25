using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManagerTest : MonoBehaviour
{

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        
    }

    public void StartDialogue (DialogueTesting dialogueTesting)
    {
        nameText.text = dialogueTesting.name;

        sentences.Clear();
        foreach (string sentence in dialogueTesting.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
    
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
    }
}
