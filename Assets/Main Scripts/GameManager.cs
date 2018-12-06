using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    
    public KeyCode restartGameKey = KeyCode.R;
    public KeyCode restartSceneKey = KeyCode.T;

	// Use this for initialization
	void Awake () {
		if (instance == null) // if there is no GameManager do not destroy on load
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // if there is another GameManager... destroy yourself 
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(restartGameKey))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(restartSceneKey))
        {
            RestartScene();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Debug.Log("Restarting Game... ");
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting This Scene...");
    }

    public void endGame()
    {
        // When the score is higher than our previous best, record a new high score.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Starting Game");
    }

}
