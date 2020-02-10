using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyTarget2 : Interactable
{
    public DialogueTrigger DT;
    public override void Interact()
    {
        Debug.Log("Tiny's dialogue is " + DT.index + " It should be 2");
        //if Tiny talks to Ms. Delilah then Tiny's next dialogue is triggered
        if (DT.index == 2)
        {
            DT.index = 3; //change this to whatever the index the dialogue is
        }

        base.Interact();
    }
}
