using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]

public class MissionBehavior : ScriptableObject
{
    public void missionEvent()
    {
        Debug.Log("Test Mission successful");
    }
}
