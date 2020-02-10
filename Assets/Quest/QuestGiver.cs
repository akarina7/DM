using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Quest[] quests;
    
    //Debugging purpose:
    [SerializeField]
    private QuestLog tmpLog;

    public GameObject questLogWindow;

    private void Awake()
    {
        tmpLog.AcceptQuest(quests[0]);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            questLogWindow.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            questLogWindow.SetActive(false);
        }
    }
}
