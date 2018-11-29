using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WorldCanvasMenu : MonoBehaviour, IPointerClickHandler 
{

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Clicked" + gameObject.name);
		QuitGame();
	}
	
	public void QuitGame()
	{
		Debug.Log("Quiting Game...");
		Application.Quit();
	}
}
