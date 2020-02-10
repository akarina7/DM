using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactRange = 2;

    void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, GameManager.instance.player.position) < interactRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Interact();
            }
        }
        
    }

    public virtual void Interact()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
