using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayer : MonoBehaviour {

	[SerializeField]
	private NPCMove theNPC;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			theNPC.seePlayer = true;
		}
		Debug.Log ("Enter " + other.gameObject.name);
	}

	void OnTriggerExit (Collider other)
	{
		if (other.CompareTag("Player")) {
			theNPC.seePlayer = false;
			theNPC.waiting = true;
		}
		Debug.Log ("Exit " + other.name);
	}
}
