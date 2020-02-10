using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTransition : MonoBehaviour
{
    public GameObject Fader;
    public Vector2 teleportToPosition;
    private Animator anim;
    public float fadeWait;
    private string sceneName;
    
    private void Awake()
    {
        if (Fader != null)
        {
            anim = Fader.GetComponent<Animator>();
            Fader.SetActive(true);
        }
    }

    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (DialogueManager.instance.inDialogue)
        {
            Fader.SetActive(false);
        }
        if (DialogueManager.instance.getEndOfDialogue() && (sceneName ==  "Intro" || sceneName == "Mafia_Base2"))
        {
            Fader.SetActive(true);

            StartCoroutine(delayLoadScene());
        }
    }
    
    IEnumerator delayLoadScene()
    {
        yield return new WaitForSeconds(0);
        
        if (sceneName == "Intro")
        {
            SceneManager.LoadScene("Mafia_Base2");

        }
        else if (sceneName == "Mafia_Base2")
        {
            Debug.Log("inside delayloadscene and mafia base 2");
            SceneManager.LoadScene("DM_Overworld_RankC");
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
//        Fader.SetActive(true);
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            FadeOut();
            Invoke("GoToDestination",0.5f);
            Invoke("FadeIn", fadeWait);
            //SceneManager.LoadScene(SceneToLoad);
        }
    }

    public void GoToDestination()
    {
        GameManager.instance.player.position = teleportToPosition;
    }

    public void FadeOut()
    {
        anim.SetBool("FadeI", false);
        anim.SetBool("FadeO", true);
    }

    public void FadeIn()
    {
        anim.SetBool("FadeO", false);
        anim.SetBool("FadeI", true);
    }
}
