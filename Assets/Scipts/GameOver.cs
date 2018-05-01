using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	[SerializeField]
	private GameObject[] goButtons;
	[SerializeField]
	private Image fadeImage;
	[SerializeField]
	private float fadeInTime;

	private float fadeInTimer = 0;
	private bool fadedIn = false;
	private int selectedButton = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fadedIn) {
			if (Input.GetAxis ("Vertical") >= 0.01f) {
				selectedButton = (int)Mathf.Max (0, selectedButton - 1);
				EventSystem.current.SetSelectedGameObject (goButtons [selectedButton]);
			} else if (Input.GetAxis ("Vertical") <= -0.01f) {
				selectedButton = (int)Mathf.Min (goButtons.Length - 1, selectedButton + 1);
				EventSystem.current.SetSelectedGameObject (goButtons [selectedButton]);
			}

			if (Input.GetAxis ("Select") >= 0.01f) {
				EventSystem.current.currentSelectedGameObject.GetComponent<Button> ().onClick.Invoke ();
			}
		} else {
			fadeInTimer = Mathf.Min(fadeInTimer + Time.deltaTime, fadeInTime);
			fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1.0f - (fadeInTimer / fadeInTime));
			if (fadeInTimer >= fadeInTime) {
				fadedIn = true;
				EventSystem.current.SetSelectedGameObject (goButtons [0]);
			}
		}
	}

	public void ReturnToTitle(){
		
	}

	public void Quit(){
		Application.Quit ();
	}
}
