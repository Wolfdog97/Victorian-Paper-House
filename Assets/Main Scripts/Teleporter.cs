using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	public enum TeleType
	{
		SceneChanger,
		NextScene,
		PositionMover
	}

	public TeleType teleType;

	public int sceneToChanageTo;

	public GameObject teleportTo;

	private GameObject _target;

	void Start () {
		_target = GameObject.FindWithTag("Player");
	}
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_target)
		{
			switch (teleType)
			{
				case TeleType.SceneChanger:
					Debug.Log("Changing Scene");
					ChangeScene();
					break;
				case TeleType.NextScene:
					NextScene();
					break;
				case TeleType.PositionMover:
					Debug.Log("Moving Player");
					MovePosition(_target);
					break;
			}	
		}
	}

	void ChangeScene()
	{
		SceneManager.LoadScene(sceneToChanageTo);
	}

	void NextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
	}

	void MovePosition(GameObject obj)
	{
		if (teleportTo != null)
		{
			obj.transform.position = teleportTo.transform.position;
		}
		else
		{
			Debug.LogWarning("No location set! ");
		}
		
	}
}
