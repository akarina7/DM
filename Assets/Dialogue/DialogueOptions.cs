using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue Option", menuName = "DialogueOptions")]

public class DialogueOptions : DialogueBase
{
    [TextArea(2,10)]
    public string questionText;
    
    [System.Serializable]
    public class Option
    {
        public string buttonName;
        public UnityEvent missionEvent;
    }

    public Option[] optionsInfo;
    
}
