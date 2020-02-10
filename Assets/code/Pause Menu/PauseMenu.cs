using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool _GamePaused = false;
    public GameObject _PauseMenuUI;

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(_GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        _PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _GamePaused = false;
    }

    public void Pause()
    {
        _PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _GamePaused = true;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
