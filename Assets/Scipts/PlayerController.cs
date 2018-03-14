using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

	//public float speed = 1.0f;
	//public float rotationFactor = 15.0f;

	private FirstPersonController playerMovement;
	private GameDriver gameDriver;
	private bool inDialogueRange = false;
	private bool selectKeyPressed =false;
	private bool questKeyPressed = false;
	private bool horizontalPressed = false;

	// Use this for initialization
	void Start () {
		gameDriver = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameDriver> ();
		playerMovement = gameObject.GetComponent<FirstPersonController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (selectKeyPressed) {
			if (Input.GetAxis ("Select") == 0) {
				selectKeyPressed = false;
			}
		}
		if (questKeyPressed) {
			if (Input.GetAxis ("QuestMenu") == 0) {
				questKeyPressed = false;
			}
		}
		switch (gameDriver.gameState) {
		case GameState.OverWorld:
			if (Input.GetAxis ("Select") != 0 && !selectKeyPressed && inDialogueRange) {
				selectKeyPressed = true;
				gameDriver.StartDialogue();
				break;
			}
			if (Input.GetAxis ("QuestMenu") != 0 && !questKeyPressed) {
				questKeyPressed = true;
				gameDriver.OpenQuestMenu();
				break;
			}
			break;
		
		case GameState.Dialogue:
			if (!horizontalPressed) {
				if (Input.GetAxisRaw ("Horizontal") > 0.001) {
					gameDriver.ChangeHoveredOption (1);
					horizontalPressed = true;
				} else if (Input.GetAxisRaw ("Horizontal") < -0.001) {
					gameDriver.ChangeHoveredOption (-1);
					horizontalPressed = true;
				}
			} else if(Input.GetAxis("Horizontal") == 0){
				horizontalPressed = false;
			}

			if (!selectKeyPressed) {
				if (Input.GetAxis ("Select") > 0) {
					selectKeyPressed = true;
					gameDriver.AdvanceDialogue ();
				}
			} 
			break;

		case GameState.QuestMenu:
			if (Input.GetAxis ("QuestMenu") != 0 && !questKeyPressed) {
				questKeyPressed = true;
				gameDriver.CloseQuestMenu ();
				break;
			}
			break;
		}
	}

	public void SetMovement(bool canMove){
		playerMovement.enabled = canMove;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "NPC") {
			gameDriver.NPCTarget = other.gameObject.GetComponent<NPCScript> ();
			inDialogueRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "NPC") {
			if (other.gameObject.GetComponent<NPCScript> () == gameDriver.NPCTarget) {
				inDialogueRange = false;
			}
		}
	}
}
