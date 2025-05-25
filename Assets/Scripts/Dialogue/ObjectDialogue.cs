using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour
{
    [SerializeField] private GameObject panelObject;
    [TextArea(2, 5)]
    public List<string> sentences;

    private ObjectRandomizer randomizer;
    private string cursorType;

    private void Start()
    {
        if (panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }

    public List<string> GetSentences()
    {
        return sentences;
    }

    public void Init(ObjectRandomizer randomizerRef, string cursor)
    {
        randomizer = randomizerRef;
        cursorType = cursor;
    }

    public void EndSentence()
    {
        /*
        if (panelObject != null)
        {
            panelObject.GetComponent<ObjectChoicePanel>().Initialize(this);
        }
        else
        {
            Debug.LogWarning("Panel object not assigned on " + gameObject.name);
        }
        */
    }

    public void OnTake()
    {
        Debug.Log("Object taken: " + gameObject.name);

        if (randomizer != null)
        {
            randomizer.RemoveObject(gameObject);
            gameObject.SetActive(false);
            randomizer.AssignRandom();
        }
    }

    public void OnLeave()
    {
        Debug.Log("Object left: " + gameObject.name);
    }
}
