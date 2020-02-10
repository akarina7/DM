using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgioneTarget1 : Interactable
{
    public DialogueTrigger DT;
    public DialogueTrigger Tiny;
    public override void Interact()
    {
        //Corgione's dialogue changes only if Tiny's dialogue was the last index 
        if (DT.index == 0 && Tiny.index == 3)
        {
            Debug.Log("did we enter index 3?");
            DT.index = 1; //change this to whatever the index the dialogue is
        }

        base.Interact();
    }
}
