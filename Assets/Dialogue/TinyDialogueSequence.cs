using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyDialogueSequence : MonoBehaviour
{
    public DialogueTrigger DT;
    // Update is called once per frame
    void Update()
    {
        //if Will talks to Tiny again after receiving the first dialogue (note) then
        //Tiny keeps repeating to Will to continue searching for what he needs to
        if (DT.nextDialogueOnInteract && DT.index == 2)
        {
            DT.nextDialogueOnInteract = false;
        }
    }
}
