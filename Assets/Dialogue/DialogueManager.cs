using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Instance equals to null" + gameObject.name);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject dialogueBox;
    
    public TextMeshProUGUI dialogueCharacterName;
    public TextMeshProUGUI dialogueCharacterScript;
    public Image dialogueCharacterImage;
    public float delay = 0.001f;

    public Queue<DialogueBase.characterInfo> characterInfo;

    //options
    private bool isDialogueOption;
    public GameObject dialogueOptionUI;
    public bool inDialogue;
    public GameObject[] optionButtons;
    private int numOfOptions;
    public TextMeshProUGUI questionText;

    private bool isCurrentlyTyping;
    private string completeText;

    private bool buffer;
    
    private bool reachedEndOfDialogue;

    public bool getEndOfDialogue()
    {
        return reachedEndOfDialogue;
    }
    
    private void Start()
    {
        characterInfo = new Queue<DialogueBase.characterInfo>();
    }
    public void enqueueDialogue(DialogueBase db)
    {
        if (inDialogue) return;
        inDialogue = true;
        buffer = true;
        StartCoroutine(BufferTimer());
        
        dialogueBox.SetActive(true);
        characterInfo.Clear();
        
        OptionsParser(db);
        
        foreach (DialogueBase.characterInfo info in db.dialogueInfo)
        {
            characterInfo.Enqueue(info);
        }
        dequeueDialogue();
    }

    public void dequeueDialogue()
    {
        if (isCurrentlyTyping)
        {
            if (buffer) return;
            CompleteText();
            StopAllCoroutines();
            isCurrentlyTyping = false;
            return;
        }
        
        if (characterInfo.Count == 0)
        {
            endDialogue();
            return;
        }
        
        DialogueBase.characterInfo info = characterInfo.Dequeue();
        completeText = info.characterScript;
        
        dialogueCharacterName.text = info.characterName;
        dialogueCharacterScript.text = info.characterScript;
        dialogueCharacterImage.sprite = info.characterImage;

        dialogueCharacterScript.text = "";
        StartCoroutine(TypeText(info));
    }

    IEnumerator TypeText(DialogueBase.characterInfo info)
    {
        isCurrentlyTyping = true;
        foreach (char c in info.characterScript.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueCharacterScript.text += c;
        }

        isCurrentlyTyping = false;
    }

    IEnumerator BufferTimer()
    {
        yield return new WaitForSeconds(0.1f);
        buffer = false;
    }
    public void endDialogue()
    {
        dialogueBox.SetActive(false);
        OptionsBranch();
        reachedEndOfDialogue = true;
    }

    private void CompleteText()
    {
        dialogueCharacterScript.text = completeText;
    }

    private void OptionsBranch()
    {
        if (isDialogueOption) 
        {
            dialogueOptionUI.SetActive(true);
        }
        else
        {
            inDialogue = false;
        }
    }

    public void closeOptions()
    {
        dialogueOptionUI.SetActive(false);
    }

    private void OptionsParser(DialogueBase db)
    {
        if (db is DialogueOptions)
        {
            isDialogueOption = true;
            DialogueOptions dialgoueOption = db as DialogueOptions;
            numOfOptions = dialgoueOption.optionsInfo.Length;
            questionText.text = dialgoueOption.questionText;

//            optionButtons[0].GetComponent<Button>().Select(); //select option with keyboard
            
            for (int i = 0; i < numOfOptions; i++)
            {
                optionButtons[i].SetActive(true);
                optionButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    dialgoueOption.optionsInfo[i].buttonName;
                MissionHandler handleEvent = optionButtons[i].GetComponent<MissionHandler>();
                handleEvent.EventHandler = dialgoueOption.optionsInfo[i].missionEvent;
            }
        }
        else
        {
            isDialogueOption = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inDialogue)
            {
                dequeueDialogue();
            }
        }
    }
}
