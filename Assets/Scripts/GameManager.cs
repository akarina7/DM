using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Transform player;
//    [SerializeField] private Will_Movement will_player;
//    private NPC currentTarget;

//    void Update()
//    {
//        ClickTarget();
//    }
//    private void ClickTarget()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 256);
//
//            if (hit.collider != null)
//            {
//                if (currentTarget != null)
//                {
//                    currentTarget.DeSelect();
//                }
//
//                currentTarget = hit.collider.GetComponent<NPC>();
//
//                will_player.MyTarget = currentTarget.Select();
//            }
//            else
//            {
//                if (currentTarget != null)
//                {
//                    currentTarget.DeSelect();
//                }
//
//                currentTarget = null;
//                will_player.MyTarget = null;
//            }
//        }
//    }
}
