using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public void LoadLevelName(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}
	
	public void LoadLevelIndex(int levelIndex)
	{
		SceneManager.LoadScene(levelIndex);
	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
	}
 
	public void LoadSceneZero()
	{
		SceneManager.LoadScene(0);
	}
}
