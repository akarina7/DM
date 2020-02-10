using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CharacterRemoved();
public class NPC : Character
{
    public event CharacterRemoved characterRemoved;
    public virtual void DeSelect()
    {
        
    }

    public virtual Transform Select()
    {
        return hitBox;
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }
}
