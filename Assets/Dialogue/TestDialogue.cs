using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this was just to test the dialogue, it shouldn't actually be attached to unity since it will appear once you hit the space bar
public class TestDialogue : MonoBehaviour
{
    public DialogueBase dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.instance.enqueueDialogue(dialogue);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerDialogue();
        }
    }
}
