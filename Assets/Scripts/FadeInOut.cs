using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;
    
    public string SceneToLoad;
//    public Vector2 playerPosition;
//    public VectorValue playerStorage;
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

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
        
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero,Quaternion.identity) as GameObject;
            Destroy(panel,1);
        }
    }

    public void startFade(string scene)
    {
        SceneToLoad = scene;
        Debug.Log("inside fadeio function");
        StartCoroutine(FadeCo());
    }

//    public void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !other.isTrigger)
//        {
//            playerStorage.initialValue = playerPosition;
//            StartCoroutine(FadeCo());
//            //SceneManager.LoadScene(SceneToLoad);
//        }
//    }

    public IEnumerator FadeCo()
    {
        if(fadeOutPanel != null) 
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}