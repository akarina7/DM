using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }
    
    [SerializeField]
    private Button[] actionButtons;

    private KeyCode action1, action2, action3;

    void Start()
    {
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(1);
        }
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(2);
        }

        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(3);
        }

        //this button will open / close all bags
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
    }

    private void ActionButtonOnClick(int btnIndex)
    {
        actionButtons[btnIndex].onClick.Invoke();
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount == 0)
        {
            Debug.Log("testing");
            clickable.MyIcon.color = new Color(0,0,0,0);
        }
    }
}
