using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerTest : MonoBehaviour
{
    public DialogueTesting dialogueTesting;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManagerTest>().StartDialogue(dialogueTesting);
    }
}
