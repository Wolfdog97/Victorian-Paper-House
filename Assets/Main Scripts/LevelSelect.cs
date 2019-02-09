using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{

	public GameObject loadingScreen;
	public Slider slider;
	public TextMeshProUGUI progressText;
	
	public void LoadLevelName(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}
	
	public void LoadLevelIndex(int sceneIndex)
	{
		//SceneManager.LoadScene(sceneIndex);
		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
	}
 
	public void LoadSceneZero()
	{
		SceneManager.LoadScene(0);
	}

	IEnumerator LoadAsynchronously(int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

		loadingScreen.SetActive(true);
		
		while ( (!operation.isDone))
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);

			slider.value = progress;
			progressText.text = progress * 100f + "%";
			
			Debug.Log(progress);
			yield return null;
		}
	}
}
