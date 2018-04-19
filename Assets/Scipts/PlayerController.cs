﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

	//public float speed = 1.0f;
	//public float rotationFactor = 15.0f;

	private FirstPersonController playerMovement;
	private QuestObject questObjectTarget = null;
	private GameDriver gameDriver;
	private bool inDialogueRange = false;
	private bool inCollectRange = false;
	private bool selectKeyPressed =false;
	private bool questKeyPressed = false;
	private bool horizontalPressed = false;
	private bool verticalPressed = false;

	// Use this for initialization
	void Start () {
		gameDriver = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameDriver> ();
		playerMovement = gameObject.GetComponent<FirstPersonController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (selectKeyPressed) {
			if (Input.GetAxisRaw ("Select") == 0) {
				selectKeyPressed = false;
			}
		}
		if (questKeyPressed) {
			if (Input.GetAxisRaw ("QuestMenu") == 0) {
				questKeyPressed = false;
			}
		}
		if (horizontalPressed) {
			if (Input.GetAxisRaw ("Horizontal") == 0) {
				horizontalPressed = false;
			}
		}
		if (verticalPressed) {
			if (Input.GetAxisRaw ("Vertical") == 0) {
				verticalPressed = false;
			}
		}
		switch (gameDriver.gameState) {
		case GameState.OverWorld:
			if (Input.GetAxis ("Select") != 0 && !selectKeyPressed) {
				selectKeyPressed = true;
				if (inDialogueRange) {
					gameDriver.StartDialogue ();
				} else if (inCollectRange) {
					questObjectTarget.Collect ();
				}
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
			} else if(Input.GetAxisRaw("Horizontal") == 0){
				horizontalPressed = false;
			}

			if (!selectKeyPressed) {
				if (Input.GetAxisRaw ("Select") > 0) {
					selectKeyPressed = true;
					gameDriver.AdvanceDialogue ();
				}
			} 
			break;

		case GameState.QuestMenu:
			if (!GameDriver.Instance.questMenu.IsFlipping) {
				if (Input.GetAxisRaw ("QuestMenu") != 0) {
					if (!questKeyPressed) {
						questKeyPressed = true;
						gameDriver.CloseQuestMenu ();
						break;
					}
				}

				if (!verticalPressed) {
					if (Input.GetAxisRaw ("Vertical") >= 0.001) {
						GameDriver.Instance.questMenu.ChangeSelected (SelectDir.Up);
						verticalPressed = true;
					} else if (Input.GetAxisRaw ("Vertical") <= -0.001) {
						GameDriver.Instance.questMenu.ChangeSelected (SelectDir.Down);
						verticalPressed = true;
					}
				}

				if (!horizontalPressed) {
					if (Input.GetAxisRaw ("Horizontal") >= 0.001) {
						GameDriver.Instance.questMenu.TurnPage (TurnDir.Foward);
						horizontalPressed = true;
					} else if (Input.GetAxisRaw ("Horizontal") <= -0.001) {
						GameDriver.Instance.questMenu.TurnPage (TurnDir.Back);	
						horizontalPressed = true;
					}
				}
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
		if (other.gameObject.tag == "QuestItem") {
			questObjectTarget = other.gameObject.GetComponent<QuestObject> ();
			inCollectRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "NPC") {
			if (other.gameObject.GetComponent<NPCScript> () == gameDriver.NPCTarget) {
				inDialogueRange = false;
			}
		}
		if (other.gameObject.tag == "QuestItem") {
			if (questObjectTarget == other.gameObject.GetComponent<QuestObject> ()) {
				questObjectTarget = null;
				inCollectRange = false;
			}
		}
	}
}
