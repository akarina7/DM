using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTarget : Interactable
{
    public DialogueTrigger DT;
    public override void Interact()
    {
        Debug.Log("Before" + DT.index);
        DT.index = 1; //change this to whatever the index the dialogue is
        Debug.Log("After" + DT.index);

        base.Interact();
    }
}
