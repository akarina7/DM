using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueTrigger : Interactable
{
    public DialogueBase[] DB;
    [HideInInspector] public int index;
    public bool nextDialogueOnInteract;
    private string sceneName;

    public void Start()
    {
        if (nextDialogueOnInteract)
        {
            index = -1;
        }
        else
        {
            index = 0;
        }
        //this checks the scene for the intro

        Scene currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;
        
        if (sceneName == "Intro")
        {
            StartCoroutine(delayDialogue());
        }
        //this checks the scene for the mafia_base2
        else if (sceneName == "Mafia_Base2")
        {
            StartCoroutine(delayDialogue());
            Debug.Log("entered mafia base dialogue");
        }
        
    }

    IEnumerator delayDialogue()
    {
        yield return new WaitForSeconds(1);
        DialogueManager.instance.enqueueDialogue(DB[0]);
    }
    
    public override void Interact()
    {
        Debug.Log("Interacted! Index " + index);
        if (nextDialogueOnInteract && !DialogueManager.instance.inDialogue)
        {
            Debug.Log("entered dialoguetrigger interact");
            if (index < DB.Length - 1)
            {
                index++;
            }
        }
        DialogueManager.instance.enqueueDialogue(DB[index]);

    }
    
    //this is used to change the index of the dialogue
    //first attach the generic interact to the object it should interact with to trigger index change
    //Create a new event
    //Drag the NPC that dialogue should change in inspector
    //Change No Function to DialogueTrigger > SetIndex
    public void SetIndex(int i)
    {
        index = i;
    }

}
