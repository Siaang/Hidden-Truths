using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Click_Handler : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                DialogueTriggerTest trigger = hit.collider.GetComponent<DialogueTriggerTest>();
                if (trigger != null)
                {
                    trigger.TriggerDialogue();
                }
            }
        }      
    }
  
}
