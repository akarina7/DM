using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MissionHandler : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent EventHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(InDialogueBuffer());
        //throw new System.NotImplementedException();
    }

    IEnumerator InDialogueBuffer()
    {
        yield return new WaitForSeconds(0.01f);
        EventHandler.Invoke();
        DialogueManager.instance.closeOptions();
        DialogueManager.instance.inDialogue = false; //this will open again if we hit space
        
    }
}
