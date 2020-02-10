using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Little_GirlQuest : Interactable
{
    public DialogueTrigger DT;
    
    public override void Interact()
    {
        //If Will talks to the little girl again after receiving the first dialogue then it won't continue until the ball is found
        if (DT.index == 1)
        {
//            DT.index = 1; //change this to whatever the index the dialogue is
            DT.nextDialogueOnInteract = false;
        }

        base.Interact();
    }
}
