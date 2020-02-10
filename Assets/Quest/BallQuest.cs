using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallQuest : MonoBehaviour
{
    private CollectObjective test;
    private void Start()
    {
//        test = GetComponent<CollectObjective>();
//        FindObjectOfType<Image>()
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("found player");
        if (collision.tag == "Player")
        {
            test.MyCurrentAmount = 1;
//            test.IncreaseItemQuest();
            //test.MyCurrentAmount = 1;
            //test.MyCurrentAmount++;
            //MyCurrentAmount++;
        }
    }
}
