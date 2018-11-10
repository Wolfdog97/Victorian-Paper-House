using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    
    public RigidbodyFirstPersonController rigidbodyFirstPersonController;
    

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        // Unlock Mouse cursor (find a better way to do this)
        rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        // Unlock Mouse cursor (find a better way to do this)
        rigidbodyFirstPersonController.mouseLook.SetCursorLock(false);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu");
        //SceneManager.LoadScene("Menu");
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quiting Game...");
        Application.Quit();
    }
}
