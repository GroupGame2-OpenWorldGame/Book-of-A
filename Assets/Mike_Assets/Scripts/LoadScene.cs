using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	[SerializeField]
	private string sceneToLoad;

	public void NextScene()
	{
		SceneManager.LoadScene (sceneToLoad);
	}
}
