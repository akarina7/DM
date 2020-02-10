using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteract : Interactable
{
    public UnityEvent events;

    //In Inspector:
    //add new event:
    //Example to set a game object to disappear:
    //First attach Generic Interact to the object it should interact with
    //Drag the object it should disappear in inspector
    //GameObject.SetActive should be unchecked
    public override void Interact()
    {
        if(events != null) events.Invoke();
        base.Interact();
    }
}
