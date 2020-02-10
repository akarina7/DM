using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyTarget1 : Interactable
{
    public DialogueTrigger DT;
    public override void Interact()
    {
        //if Tiny talks to Al Corgione then Tiny's next dialogue is triggered
        if (DT.index == 0)
        {
//            DT.index = 1; //change this to whatever the index the dialogue is
            DT.nextDialogueOnInteract = true;
        }

        base.Interact();
    }
}