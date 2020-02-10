using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueBase : ScriptableObject
{
    [System.Serializable]
    public class characterInfo
    {
        public string characterName;
        public Sprite characterImage;
        [TextArea(4, 8)] 
        public string characterScript;
    }

    [Header("Insert Character Dialogue Below")]
    public characterInfo[] dialogueInfo;

}
