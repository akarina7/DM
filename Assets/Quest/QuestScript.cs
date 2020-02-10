using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }
    public void Start()
    {
        
    }

    public void Update()
    {
        
    }
    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        QuestLog.MyInstance.ShowDescription(MyQuest);
    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;

    }
}
