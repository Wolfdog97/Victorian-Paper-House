using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	public enum TeleType
	{
		SceneChanger,
		PositionMover
	}

	public TeleType teleType;

	public int sceneToChanageTo;

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
				case TeleType.PositionMover:
					Debug.Log("Moving Player");
					break;
			}	
		}
	}

	void ChangeScene()
	{
		SceneManager.LoadScene(sceneToChanageTo);
	}
}
