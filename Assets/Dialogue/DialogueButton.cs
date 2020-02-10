using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    public void getNextLine()
    {
        Debug.Log("Clicked the button");
        DialogueManager.instance.dequeueDialogue();
    }
}
