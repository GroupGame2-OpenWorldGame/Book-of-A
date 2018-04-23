using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginGame : MonoBehaviour
{

	//This Script will be used on the Start Game Button as a smooth transition into starting the game
	//The camera will smoothly lerp to the player's position and then be replaced by the player camera
	//The game will start either by the exposition text arriving on screen before moving to the player, or by moving to the player and then showing the Exposition text

		public Transform startMarker;
		public Transform endMarker;
		public float speed = 1.0F;
		private float startTime;
		private float journeyLength;

	//	public GameObject playerGO;
	//	public Transform Target; 
	//	public Camera mainCamera; 
		public Button startGame; 

	// Use this for initialization
	void Start ()
	{
		
		Button btn = startGame.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);

		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
	}

	public void TaskOnClick()
	{

		Debug.Log("you clicked this button, good job.");
		// Lerp the Camera to the player's position and then default to the player Camera
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
		//playerGO.SetActive (true);

	}
		

	void Update() 
	{
		
	}


}

