using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour {

	public Collider interactRange;
	public GameObject interactSprite;

	public string characterName;
	public Sprite portrait;
	public string dialogueXMLPath;
	public TextAsset dialogueText;

	public DialogueHead dialogueHead;

	private GameDriver gameDriver;

	private bool playerInRange = false;

	// Use this for initialization
	void Start () {
		//dialogueHead.SetHeadToLines ();
		//dialogueHead.SetLines ();



		Debug.Log (dialogueHead.FirstLineId);
		Debug.Log (dialogueHead.NPCName);

		interactSprite.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetPlayerInRange(bool inRange){
		playerInRange = inRange;
		interactSprite.SetActive(inRange);
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			SetPlayerInRange (true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			SetPlayerInRange (false);
		}
	}
}
